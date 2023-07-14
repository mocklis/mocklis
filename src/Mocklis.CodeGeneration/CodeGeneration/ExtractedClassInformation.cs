// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtractedClassInformation.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Mocklis.CodeGeneration.Compatibility;
    using Mocklis.CodeGeneration.UniqueNames;

    #endregion

    public record struct MockSettings(bool MockReturnsByRef, bool MockReturnsByRefReadonly, bool Strict, bool VeryStrict);

    public class ExtractionMocklisSymbols
    {
        private readonly Compilation _compilation;
        private readonly INamedTypeSymbol _object;
        public INamedTypeSymbol MocklisClassAttribute { get; }

        public ExtractionMocklisSymbols(Compilation compilation)
        {
            INamedTypeSymbol GetTypeSymbol(string metadataName)
            {
                return compilation.GetTypeByMetadataName(metadataName) ??
                       throw new ArgumentException($"Compilation does not contain {metadataName}.", nameof(compilation));
            }

            _compilation = compilation;
            _object = GetTypeSymbol("System.Object");
            MocklisClassAttribute = GetTypeSymbol("Mocklis.Core.MocklisClassAttribute");
        }

        public bool HasImplicitConversionToObject(ITypeSymbol symbol)
        {
            return _compilation.HasImplicitConversion(symbol, _object);
        }
    }

    public class ExtractedClassInformation
    {
        public INamedTypeSymbol ClassSymbol { get; }
        public MockSettings Settings { get; }
        public bool NullableContextEnabled { get; }
        private readonly ExtractedInterfaceInformation[] _interfaces;
        public IReadOnlyList<ExtractedInterfaceInformation> Interfaces => _interfaces;

        private ExtractedClassInformation(INamedTypeSymbol classSymbol, IEnumerable<ExtractedInterfaceInformation> interfaces, MockSettings settings, bool nullableContextEnabled)
        {
            ClassSymbol = classSymbol;
            Settings = settings;
            NullableContextEnabled = nullableContextEnabled;
            _interfaces = interfaces.ToArray();
        }

        public static ExtractedClassInformation BuildFromClassSymbol(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel, CancellationToken cancellationToken)
        {
            var symbols = new ExtractionMocklisSymbols(semanticModel.Compilation);

            var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) ?? throw new ArgumentException("symbol for class was not found in semantic model.", nameof(classDeclaration));

            var settings = GetSettingsFromAttribute(classDeclaration, classSymbol, symbols);

            var classIsInNullableContext = semanticModel.ClassIsInNullableContext(classDeclaration);

            // Identify the interfaces here...
            var namesToReserveAndUse = new List<string>(classSymbol.BaseType?.GetUsableNames() ?? Array.Empty<string>()) { classSymbol.Name };
            var uniquifier = new Uniquifier(namesToReserveAndUse);

            var interfaceList = new List<(INamedTypeSymbol interfaceSymbol, List<ISymbol> memberSymbols)>();

            var baseTypeSymbol = classSymbol.BaseType;
            foreach (var interfaceSymbol in classSymbol.AllInterfaces)
            {
                var memberSymbols = new List<ISymbol>();

                foreach (var memberSymbol in interfaceSymbol.GetMembers())
                {
                    if (!memberSymbol.IsAbstract)
                    {
                        continue;
                    }

                    if (memberSymbol is IMethodSymbol && !memberSymbol.CanBeReferencedByName)
                    {
                        continue;
                    }

                    if (baseTypeSymbol?.FindImplementationForInterfaceMember(memberSymbol) != null)
                    {
                        continue;
                    }

                    memberSymbols.Add(memberSymbol);
                    uniquifier.ReserveName(memberSymbol.Name);
                }

                if (memberSymbols.Any())
                {
                    interfaceList.Add((interfaceSymbol, memberSymbols));
                }
            }

            var interfaces = new ExtractedInterfaceInformation[interfaceList.Count];

            for (int i = 0; i < interfaceList.Count; i++)
            {
                var (isym, msyms) = interfaceList[i];

                var members = new IMemberMock[msyms.Count];

                for (int j = 0; j < msyms.Count; j++)
                {
                    // Create the right type of member...?
                    members[j] = CreateMock(symbols, classSymbol, msyms[j], uniquifier, settings);
                }

                interfaces[i] = new ExtractedInterfaceInformation(isym, members);
            }

            return new ExtractedClassInformation(classSymbol, interfaces, settings, classIsInNullableContext);
        }


        public static IMemberMock CreateMock(ExtractionMocklisSymbols mocklisSymbols, INamedTypeSymbol classSymbol, ISymbol memberSymbol, Uniquifier uniquifier, MockSettings settings)
        {
            string mockMemberName = uniquifier.GetUniqueName(memberSymbol.MetadataName);

            switch (memberSymbol)
            {
                case IPropertySymbol memberPropertySymbol:
                {
                    var hasRestrictedParameter = memberPropertySymbol.Parameters.Any(p => !mocklisSymbols.HasImplicitConversionToObject(p.Type));
                    var hasRestrictedReturnType = !mocklisSymbols.HasImplicitConversionToObject(memberPropertySymbol.Type);

                    bool useVirtualMethod = hasRestrictedParameter || hasRestrictedReturnType ||
                                            memberPropertySymbol.ReturnsByRef && !settings.MockReturnsByRef ||
                                            memberPropertySymbol.ReturnsByRefReadonly && !settings.MockReturnsByRefReadonly;

                    if (memberPropertySymbol.IsIndexer)
                    {
                        if (useVirtualMethod)
                        {
                            return NullMemberMock.Instance;
                            // return new VirtualMethodBasedIndexerMock(memberPropertySymbol, mockMemberName);
                        }

                        return NullMemberMock.Instance;
                        // return new PropertyBasedIndexerMock(memberPropertySymbol, mockMemberName);
                    }

                    if (useVirtualMethod)
                    {
                        return NullMemberMock.Instance;
                        // return new VirtualMethodBasedPropertyMock(memberPropertySymbol, mockMemberName);
                    }

                    return NullMemberMock.Instance;
                    // return new PropertyBasedPropertyMock(memberPropertySymbol, mockMemberName);
                }

                case IEventSymbol memberEventSymbol:
                    return NullMemberMock.Instance;
                    // return new PropertyBasedEventMock(memberEventSymbol, mockMemberName);
                case IMethodSymbol memberMethodSymbol:
                {
                    var hasRestrictedParameter = memberMethodSymbol.Parameters.Any(p => !mocklisSymbols.HasImplicitConversionToObject(p.Type));
                    var hasRestrictedReturnType = !memberMethodSymbol.ReturnsVoid &&
                                                  !mocklisSymbols.HasImplicitConversionToObject(memberMethodSymbol.ReturnType);

                    bool useVirtualMethod = hasRestrictedParameter || hasRestrictedReturnType || memberMethodSymbol.IsVararg;

                    useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRef && !settings.MockReturnsByRef;
                    useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRefReadonly && !settings.MockReturnsByRefReadonly;

                    var typeParameterSubstitutions = new Substitutions(classSymbol, memberMethodSymbol);

                    if (useVirtualMethod)
                    {
                        return NullMemberMock.Instance;
                        // return new VirtualMethodBasedMethodMock(memberMethodSymbol, mockMemberName, typeParameterSubstitutions);
                    }

                    if (memberMethodSymbol.Arity > 0)
                    {
                        var metadataName = memberSymbol.MetadataName;
                        var mockProviderName = uniquifier.GetUniqueName( "_" + char.ToLowerInvariant(metadataName[0]) + metadataName.Substring(1));
                        return NullMemberMock.Instance;
                        // return new PropertyBasedMethodMockWithTypeParameters(memberMethodSymbol, mockMemberName, typeParameterSubstitutions, mockProviderName);
                    }

                    return NullMemberMock.Instance;
                    // return new PropertyBasedMethodMock(memberMethodSymbol, mockMemberName, typeParameterSubstitutions);
                }

                default:
                    return NullMemberMock.Instance;
            }
        }

        public static MockSettings GetSettingsFromAttribute(ClassDeclarationSyntax classDeclaration, INamedTypeSymbol classSymbol, ExtractionMocklisSymbols symbols)
        {
            bool mockReturnsByRef = false;
            bool mockReturnsByRefReadonly = true;
            bool strict = false;
            bool veryStrict = false;
            var attribute = classSymbol.GetAttributes().SingleOrDefault(a =>
                a.AttributeClass != null && a.AttributeClass.Equals(symbols.MocklisClassAttribute, SymbolEqualityComparer.Default));

            if (attribute != null)
            {
                var attributeSyntaxReference = attribute.ApplicationSyntaxReference ??
                                               throw new ArgumentException("MocklisClass attribute did not have syntax reference.");

                var attributeArguments = classDeclaration.FindNode(attributeSyntaxReference.Span).DescendantNodes().OfType<AttributeArgumentSyntax>();

                foreach (var k in attributeArguments)
                {
                    if (k.Expression is LiteralExpressionSyntax les)
                    {
                        var name = k.NameEquals?.Name.Identifier.Text;
                        bool value;
                        if (les.IsKind(SyntaxKind.TrueLiteralExpression))
                        {
                            value = true;
                        }
                        else if (les.IsKind(SyntaxKind.FalseLiteralExpression))
                        {
                            value = false;
                        }
                        else
                        {
                            break;
                        }

                        switch (name)
                        {
                            case "MockReturnsByRef":
                                mockReturnsByRef = value;
                                break;
                            case "MockReturnsByRefReadonly":
                                mockReturnsByRefReadonly = value;
                                break;
                            case "Strict":
                                strict = value;
                                break;
                            case "VeryStrict":
                                veryStrict = value;
                                break;
                        }
                    }
                }
            }

            // 'Very strict' implies 'strict'
            strict |= veryStrict;

            return new MockSettings(mockReturnsByRef, mockReturnsByRefReadonly, strict, veryStrict);
        }

        public void AddSourceToContext(SourceProductionContext context)
        {
            var sb = new StringBuilder();

            sb.AppendLine("// <auto-generated />");
            sb.AppendLine();

            context.AddSource(ClassSymbol.Name + ".g.cs", sb.ToString());
        }
    }

    public class ExtractedInterfaceInformation
    {
        public INamedTypeSymbol InterfaceSymbol { get; }
        private readonly IMemberMock[] _memberSymbols;
        public IReadOnlyList<IMemberMock> MemberSymbols => _memberSymbols;

        public ExtractedInterfaceInformation(INamedTypeSymbol interfaceSymbol, IReadOnlyCollection<IMemberMock> memberSymbols)
        {
            InterfaceSymbol = interfaceSymbol;
            _memberSymbols = memberSymbols.ToArray();
        }
    }
}
