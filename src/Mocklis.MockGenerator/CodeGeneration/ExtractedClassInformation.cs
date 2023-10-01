// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtractedClassInformation.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Mocklis.CodeGeneration;
using Mocklis.CodeGeneration.UniqueNames;
using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#endregion

public class ExtractedClassInformation
{
    public INamedTypeSymbol ClassSymbol { get; }
    public bool NullableEnabled { get; }
    public MockSettings Settings { get; }
    private readonly ExtractedInterfaceInformation[] _interfaces;

    private ExtractedClassInformation(INamedTypeSymbol classSymbol, IEnumerable<ExtractedInterfaceInformation> interfaces, MockSettings settings,
        bool nullableAnnotationsEnabled)
    {
        NullableEnabled = nullableAnnotationsEnabled;
        ClassSymbol = classSymbol;
        Settings = settings;
        _interfaces = interfaces.ToArray();
    }

    public static ExtractedClassInformation BuildFromClassSymbol(ClassDeclarationSyntax classDeclaration, SemanticModel semanticModel)
    {
        //TODO: Reinstate cancellation token...

        var classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration) ??
                          throw new ArgumentException("symbol for class was not found in semantic model.", nameof(classDeclaration));

        var settings = GetSettingsFromAttribute(classDeclaration, classSymbol, semanticModel.Compilation);

        bool nullableAnnotationsEnabled = false;
        var classIsInNullableContext = semanticModel.GetNullableContext(classDeclaration.Span.Start);
        if (classIsInNullableContext.AnnotationsEnabled())
        {
            nullableAnnotationsEnabled = true;
        }

        if (classIsInNullableContext.AnnotationsInherited())
        {
            var a = semanticModel.Compilation.Options.NullableContextOptions;
            nullableAnnotationsEnabled = a.AnnotationsEnabled();
        }
        // TODO: Add test case(s) for when there is compilation enabled nullable enabled and no specific annotation in the source file

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
                members[j] = CreateMock(semanticModel.Compilation, classSymbol, msyms[j], uniquifier, settings);
            }

            interfaces[i] = new ExtractedInterfaceInformation(isym, members);
        }

        return new ExtractedClassInformation(classSymbol, interfaces, settings, nullableAnnotationsEnabled);
    }

    private static IMemberMock CreateMock(Compilation compilation, INamedTypeSymbol classSymbol, ISymbol memberSymbol,
        Uniquifier uniquifier, MockSettings settings)
    {
        string mockMemberName = uniquifier.GetUniqueName(memberSymbol.MetadataName);

        var objectSymbol  = compilation.GetTypeByMetadataName("System.Object") ?? throw new InvalidOperationException("Could not find object type");

        switch (memberSymbol)
        {
            case IPropertySymbol memberPropertySymbol:
            {
                var hasRestrictedParameter = memberPropertySymbol.Parameters.Any(p => !compilation.HasImplicitConversion(p.Type, objectSymbol));
                var hasRestrictedReturnType = !compilation.HasImplicitConversion(memberPropertySymbol.Type, objectSymbol);

                bool useVirtualMethod = hasRestrictedParameter || hasRestrictedReturnType ||
                                        memberPropertySymbol.ReturnsByRef && !settings.MockReturnsByRef ||
                                        memberPropertySymbol.ReturnsByRefReadonly && !settings.MockReturnsByRefReadonly;

                if (memberPropertySymbol.IsIndexer)
                {
                    if (useVirtualMethod)
                    {
                        return new VirtualMethodBasedIndexerMock(memberPropertySymbol, mockMemberName);
                    }

                    return new PropertyBasedIndexerMock(memberPropertySymbol, mockMemberName);
                }

                if (useVirtualMethod)
                {
                    return new VirtualMethodBasedPropertyMock(memberPropertySymbol, mockMemberName);
                }

                return new PropertyBasedPropertyMock(memberPropertySymbol, mockMemberName);
            }

            case IEventSymbol memberEventSymbol:
                return new PropertyBasedEventMock(memberEventSymbol, mockMemberName);
            case IMethodSymbol memberMethodSymbol:
            {
                var hasRestrictedParameter = memberMethodSymbol.Parameters.Any(p => !compilation.HasImplicitConversion(p.Type, objectSymbol));
                var hasRestrictedReturnType = !memberMethodSymbol.ReturnsVoid &&
                                              !compilation.HasImplicitConversion(memberMethodSymbol.ReturnType, objectSymbol);

                bool useVirtualMethod = hasRestrictedParameter || hasRestrictedReturnType || memberMethodSymbol.IsVararg;

                useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRef && !settings.MockReturnsByRef;
                useVirtualMethod = useVirtualMethod || memberMethodSymbol.ReturnsByRefReadonly && !settings.MockReturnsByRefReadonly;
                
                if (useVirtualMethod)
                {
                    var typeParameterSubstitutions = ITypeParameterSubstitutions.Build(classSymbol, memberMethodSymbol);

                    return new VirtualMethodBasedMethodMock(memberMethodSymbol, mockMemberName, typeParameterSubstitutions);
                }

                if (memberMethodSymbol.Arity > 0)
                {
                    var typeParameterSubstitutions = ITypeParameterSubstitutions.Build(classSymbol, memberMethodSymbol);

                    var metadataName = memberSymbol.MetadataName;
                    var mockProviderName = uniquifier.GetUniqueName("_" + char.ToLowerInvariant(metadataName[0]) + metadataName.Substring(1));
                    return new PropertyBasedMethodMockWithTypeParameters(memberMethodSymbol, mockMemberName, typeParameterSubstitutions,
                        mockProviderName);
                }

                return new PropertyBasedMethodMock(memberMethodSymbol, mockMemberName);
            }

            default:
                return NullMemberMock.Instance;
        }
    }

    private static MockSettings GetSettingsFromAttribute(ClassDeclarationSyntax classDeclaration, INamedTypeSymbol classSymbol,
        Compilation compilation)
    {
        var mocklisClassAttribute = compilation.GetTypeByMetadataName("Mocklis.Core.MocklisClassAttribute") ?? throw new InvalidOperationException("Could not find MocklisClassAttribute type");

        bool mockReturnsByRef = false;
        bool mockReturnsByRefReadonly = true;
        bool strict = false;
        bool veryStrict = false;
        var attribute = classSymbol.GetAttributes().SingleOrDefault(a =>
            a.AttributeClass != null && a.AttributeClass.Equals(mocklisClassAttribute, SymbolEqualityComparer.Default));

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

    public void AddSourceToContext(SourceProductionContext productionContext)
    {
        var ctx = new SourceGenerationContext(Settings, ClassSymbol, NullableEnabled);

        ctx.AppendLine("// <auto-generated />");
        ctx.AppendSeparator();

        if (NullableEnabled)
        {
            ctx.AppendLine("#nullable enable");
            ctx.AppendSeparator();
        }

        var hasNamespace = !ClassSymbol.ContainingNamespace.IsGlobalNamespace;

        // TODO: Add test case for when there is no top level namespace.

        if (hasNamespace)
        {
            ctx.AppendLine($"namespace {ClassSymbol.ContainingNamespace.Name}");
            ctx.AppendLine("{");
            ctx.IncreaseIndent();
        }

        if (ClassSymbol.IsGenericType)
        {
            ctx.AppendLine($"partial class {ClassSymbol.Name}<{string.Join(", ", ClassSymbol.TypeParameters.Select(tp => tp.Name))}>");
        }
        else
        {
            ctx.AppendLine($"partial class {ClassSymbol.Name}");
        }
    
        ctx.AppendLine("{");
        ctx.IncreaseIndent();

        foreach (var i in _interfaces)
        {
            i.AddSourceForMembers(ctx);
        }

        AddSourceForConstructors(ctx);

        ctx.DecreaseIndent();
        ctx.AppendLine("}");

        if (hasNamespace)
        {
            ctx.DecreaseIndent();
            ctx.AppendLine("}");
        }

        productionContext.AddSource(ClassSymbol.FullName() + ".g.cs", ctx.ToString());
    }

    private void AddSourceForConstructors(SourceGenerationContext ctx)
    {
        var baseTypeConstructors = BaseTypeConstructors();

        if (baseTypeConstructors.Length == 1 && baseTypeConstructors[0].Parameters.Length == 0 && ctx.ConstructorStatements.Count == 0)
        {
            // This would correspond to emitting just an empty default constructor, which we don't really need to do.
            return;
        }

        var modifier = ClassSymbol.IsAbstract ? "protected" : "public";

        foreach (var constructor in baseTypeConstructors)
        {
            ctx.AppendSeparator();
            ctx.AppendLine($"{modifier} {ClassSymbol.Name}({ctx.BuildParameterList(constructor.Parameters, ITypeParameterSubstitutions.Empty)}) : base({ctx.BuildArgumentList(constructor.Parameters)})");
            ctx.AppendLine("{");
            ctx.IncreaseIndent();

            foreach (var c in ctx.ConstructorStatements)
            {
                ctx.AppendLine(c);
            }

            //var constructorDeclaration = F.ConstructorDeclaration(F.Identifier(ClassSymbol.Name))
            //    .WithModifiers(F.TokenList(F.Token(ClassSymbol.IsAbstract ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)))
            //    .WithParameterList(
            //        F.ParameterList(
            //            F.SeparatedList(constructor.Parameters.Select(tp => typesForSymbols.AsParameterSyntax(tp)))))
            //    .WithBody(F.Block(constructorStatementsWithThisWhereRequired));

            //if (parameterNames.Any())
            //{
            //    constructorDeclaration = constructorDeclaration.WithInitializer(F.ConstructorInitializer(
            //        SyntaxKind.BaseConstructorInitializer,
            //        F.ArgumentList(constructor.Parameters.AsArgumentList())));
            //}

            // declarationList.Add(constructorDeclaration);

            ctx.DecreaseIndent();
            ctx.AppendLine("}");
        }
    }

    public SyntaxList<MemberDeclarationSyntax> GenerateMembers(MocklisTypesForSymbols typesForSymbols)
    {
        var declarationList = new List<MemberDeclarationSyntax>();

        var constructorStatements = new List<StatementSyntax>();

        var constructorDeclarationList = new List<MemberDeclarationSyntax>();

        foreach (var i in _interfaces)
        {
            i.GenerateMembers(typesForSymbols, Settings, declarationList, constructorStatements, ClassSymbol);
        }

        GenerateConstructors(constructorDeclarationList, constructorStatements, typesForSymbols);

        declarationList.InsertRange(0, constructorDeclarationList);

        return new SyntaxList<MemberDeclarationSyntax>(declarationList);
    }

    private void GenerateConstructors(IList<MemberDeclarationSyntax> declarationList, IReadOnlyCollection<StatementSyntax> constructorStatements,
        MocklisTypesForSymbols typesForSymbols)
    {
        var baseTypeConstructors = BaseTypeConstructors();

        if (baseTypeConstructors.Length == 1 && baseTypeConstructors[0].Parameters.Length == 0 && constructorStatements.Count == 0)
        {
            // This would correspond to emitting just an empty default constructor, which we don't really need to do.
            return;
        }

        foreach (var constructor in baseTypeConstructors)
        {
            var parameterNames = constructor.Parameters.Select(p => p.Name).ToArray();

            var constructorStatementsWithThisWhereRequired = constructorStatements.Select(constructorStatement =>
            {
                if (constructorStatement is ExpressionStatementSyntax
                    {
                        Expression: AssignmentExpressionSyntax { Left: IdentifierNameSyntax identifierNameSyntax } assignmentExpressionSyntax
                    } expressionStatementSyntax && parameterNames.Contains(identifierNameSyntax.Identifier.Text)
                   )
                {
                    var newLeft = F.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, F.ThisExpression(), identifierNameSyntax);
                    return expressionStatementSyntax.WithExpression(assignmentExpressionSyntax.WithLeft(newLeft));
                }

                return constructorStatement;
            });

            var constructorDeclaration = F.ConstructorDeclaration(F.Identifier(ClassSymbol.Name))
                .WithModifiers(F.TokenList(F.Token(ClassSymbol.IsAbstract ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    F.ParameterList(
                        F.SeparatedList(constructor.Parameters.Select(tp => typesForSymbols.AsParameterSyntax(tp)))))
                .WithBody(F.Block(constructorStatementsWithThisWhereRequired));

            if (parameterNames.Any())
            {
                constructorDeclaration = constructorDeclaration.WithInitializer(F.ConstructorInitializer(
                    SyntaxKind.BaseConstructorInitializer,
                    F.ArgumentList(constructor.Parameters.AsArgumentList())));
            }

            declarationList.Add(constructorDeclaration);
        }
    }

    private IMethodSymbol[] BaseTypeConstructors()
    {
        static bool CanAccessConstructor(IMethodSymbol constructor)
        {
            // TODO: Should we consider allowing access to internal constructors as well - if they can be seen?
            switch (constructor.DeclaredAccessibility)
            {
                case Accessibility.Protected:
                case Accessibility.ProtectedOrInternal:
                case Accessibility.Public:
                {
                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }

        var baseTypeConstructors = ClassSymbol.BaseType?.Constructors.Where(c => !c.IsStatic && !c.IsVararg && CanAccessConstructor(c))
            .ToArray() ?? Array.Empty<IMethodSymbol>();

        return baseTypeConstructors;
    }
}

public class ExtractedInterfaceInformation
{
    public INamedTypeSymbol InterfaceSymbol { get; }
    private readonly IMemberMock[] _memberMocks;

    public ExtractedInterfaceInformation(INamedTypeSymbol interfaceSymbol, IReadOnlyCollection<IMemberMock> memberMocks)
    {
        InterfaceSymbol = interfaceSymbol;
        _memberMocks = memberMocks.ToArray();
    }

    public void GenerateMembers(MocklisTypesForSymbols typesForSymbols, MockSettings mockSettings, List<MemberDeclarationSyntax> declarationList,
        List<StatementSyntax> constructorStatements, INamedTypeSymbol classSymbol)
    {
        var interfaceNameSyntax = typesForSymbols.ParseName(InterfaceSymbol);
        foreach (var memberMock in _memberMocks)
        {
            var syntaxAdder = memberMock.GetSyntaxAdder(typesForSymbols);
            syntaxAdder.AddInitialisersToConstructor(typesForSymbols, mockSettings, constructorStatements, classSymbol.Name, InterfaceSymbol.Name);
            syntaxAdder.AddMembersToClass(typesForSymbols, mockSettings, declarationList, interfaceNameSyntax, classSymbol.Name, InterfaceSymbol.Name);
        }
    }

    public void AddSourceForMembers(SourceGenerationContext ctx)
    {
        // ctx.AppendLine($"// Add source for members in interface {InterfaceSymbol.Name}");
        foreach (var memberMock in _memberMocks)
        {
            memberMock.AddSource(ctx, InterfaceSymbol);
            ctx.AppendSeparator();
        }
    }
}
