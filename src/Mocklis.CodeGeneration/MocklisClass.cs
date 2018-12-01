// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClass.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using Microsoft.CodeAnalysis.Formatting;
    using Mocklis.CodeGeneration.UniqueNames;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public sealed class MocklisClass
    {
        private readonly SemanticModel _semanticModel;
        private readonly ClassDeclarationSyntax _classDeclaration;
        private readonly MocklisSymbols _mocklisSymbols;
        private readonly (string uniqueName, MocklisMember item)[] _interfaceMembers;
        private readonly NameSyntax _valueTuple;
        private readonly NameSyntax _action;
        private readonly string[] _problematicMembers;
        private readonly bool _isAbstract;

        public static ClassDeclarationSyntax EmptyMocklisClass(ClassDeclarationSyntax classDecl)
        {
            return classDecl.WithMembers(F.List<MemberDeclarationSyntax>())
                .WithOpenBraceToken(F.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(F.Token(SyntaxKind.CloseBraceToken));
        }

        public static ClassDeclarationSyntax UpdateMocklisClass(SemanticModel semanticModel, ClassDeclarationSyntax classDecl,
            MocklisSymbols mocklisSymbols)
        {
            var mocklisClass = new MocklisClass(semanticModel, classDecl, mocklisSymbols);
            return classDecl.WithMembers(F.List(mocklisClass.GenerateMembers()))
                .WithOpenBraceToken(F.Token(SyntaxKind.OpenBraceToken))
                .WithCloseBraceToken(F.Token(SyntaxKind.CloseBraceToken))
                .WithAdditionalAnnotations(Formatter.Annotation);
        }

        public MocklisClass(SemanticModel semanticModel, ClassDeclarationSyntax classDeclaration, MocklisSymbols mocklisSymbols)
        {
            _semanticModel = semanticModel;
            _classDeclaration = classDeclaration;

            _mocklisSymbols = mocklisSymbols;
            _valueTuple = ParseName(mocklisSymbols.ValueTuple);
            _action = ParseName(mocklisSymbols.Action);
            INamedTypeSymbol classSymbol = semanticModel.GetDeclaredSymbol(classDeclaration);
            _isAbstract = classSymbol.IsAbstract;

            List<string> problematicMembers = new List<string>();

            var interfaceMembers = FindAllMembersInClass(classSymbol, problematicMembers).ToArray();

            _problematicMembers = problematicMembers.ToArray();

            // make sure to reserve and use all names defined by the basetypes
            var uniquifier = new Uniquifier(classSymbol.BaseType.GetUsableNames());

            _interfaceMembers = uniquifier.GetUniqueNames(interfaceMembers).ToArray();
        }


        public string Name => _classDeclaration.Identifier.Text;

        private IEnumerable<MocklisMember> FindAllMembersInClass(INamedTypeSymbol classSymbol, List<string> problematicMembers)
        {
            var baseTypeSymbol = classSymbol.BaseType;

            foreach (var interfaceSymbol in classSymbol.AllInterfaces)
            {
                foreach (var memberSymbol in interfaceSymbol.GetMembers())
                {
                    if (baseTypeSymbol?.FindImplementationForInterfaceMember(memberSymbol) != null)
                    {
                        continue;
                    }

                    if (memberSymbol is IPropertySymbol memberPropertySymbol)
                    {
                        if (memberPropertySymbol.ReturnsByRef)
                        {
                            problematicMembers.Add($"{interfaceSymbol.Name}.{memberPropertySymbol.Name} (returns by reference)");
                        }
                        else if (memberPropertySymbol.ReturnsByRefReadonly)
                        {
                            problematicMembers.Add($"{interfaceSymbol.Name}.{memberPropertySymbol.Name} (returns by readonly reference)");
                        }
                        else if (memberPropertySymbol.IsIndexer)
                        {
                            yield return new MocklisIndexer(this, interfaceSymbol, memberPropertySymbol);
                        }
                        else
                        {
                            yield return new MocklisProperty(this, interfaceSymbol, memberPropertySymbol);
                        }
                    }
                    else if (memberSymbol is IEventSymbol memberEventSymbol)
                    {
                        yield return new MocklisEvent(this, interfaceSymbol, memberEventSymbol);
                    }
                    else if (memberSymbol is IMethodSymbol memberMethodSymbol && memberMethodSymbol.CanBeReferencedByName)
                    {
                        if (memberMethodSymbol.Arity > 0)
                        {
                            problematicMembers.Add($"{interfaceSymbol.Name}.{memberMethodSymbol.Name} (introduces new type parameter)");
                        }
                        else if (memberMethodSymbol.ReturnsByRef)
                        {
                            problematicMembers.Add($"{interfaceSymbol.Name}.{memberMethodSymbol.Name} (returns by reference)");
                        }
                        else if (memberMethodSymbol.ReturnsByRefReadonly)
                        {
                            problematicMembers.Add($"{interfaceSymbol.Name}.{memberMethodSymbol.Name} (returns by readonly reference)");
                        }
                        else if (memberMethodSymbol.IsVararg)
                        {
                            problematicMembers.Add($"{interfaceSymbol.Name}.{memberMethodSymbol.Name} (uses __arglist)");
                        }
                        else
                        {
                            yield return new MocklisMethod(this, interfaceSymbol, memberMethodSymbol);
                        }
                    }
                }
            }
        }

        public IEnumerable<MemberDeclarationSyntax> GenerateMembers()
        {
            var constructor = GenerateConstructor();
            if (_problematicMembers.Any())
            {
                var triviaList = F.TriviaList().Add(F.Comment("// Could not create mocks for the following members:" + Environment.NewLine));
                foreach (var problematicMember in _problematicMembers)
                {
                    triviaList = triviaList.Add(F.Comment("// * " + problematicMember + Environment.NewLine));
                }

                triviaList = triviaList.Add(F.Comment("//" + Environment.NewLine));
                triviaList = triviaList.Add(F.Comment("// Future version of Mocklis will handle these by introducing virtual members" +
                                                      Environment.NewLine));
                triviaList = triviaList.Add(F.Comment("// that can be given a 'mock' implementation in a derived class." + Environment.NewLine +
                                                      Environment.NewLine));

                constructor = constructor.WithLeadingTrivia(triviaList);
            }

            yield return constructor;

            foreach (var interfaceMember in _interfaceMembers)
            {
                yield return interfaceMember.item.MockProperty(interfaceMember.uniqueName);

                var x = interfaceMember.item.ExplicitInterfaceMember(interfaceMember.uniqueName);
                if (x != null)
                {
                    yield return x;
                }
            }
        }

        private MemberDeclarationSyntax GenerateConstructor()
        {
            List<StatementSyntax> constructorStatements = new List<StatementSyntax>();

            foreach (var interfaceMember in _interfaceMembers)
            {
                var initialisation = interfaceMember.item.InitialiseMockProperty(interfaceMember.uniqueName);
                if (initialisation != null)
                {
                    constructorStatements.Add(initialisation);
                }
            }

            return F.ConstructorDeclaration(_classDeclaration.Identifier)
                .WithModifiers(F.TokenList(F.Token(_isAbstract ? SyntaxKind.ProtectedKeyword : SyntaxKind.PublicKeyword)))
                .WithBody(F.Block(constructorStatements));
        }

        public TypeSyntax ParseTypeName(ITypeSymbol propertyType)
        {
            return F.ParseTypeName(propertyType.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
        }

        public NameSyntax ParseName(ITypeSymbol propertyType)
        {
            return F.ParseName(propertyType.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));
        }

        public NameSyntax ParseGenericName(ITypeSymbol symbol, params TypeSyntax[] typeParameters)
        {
            NameSyntax ApplyTypeParameters(NameSyntax nameSyntax)
            {
                if (nameSyntax is GenericNameSyntax genericNameSyntax)
                {
                    return genericNameSyntax.WithTypeArgumentList(F.TypeArgumentList(F.SeparatedList(typeParameters)));
                }

                if (nameSyntax is QualifiedNameSyntax qualifiedNameSyntax)
                {
                    return qualifiedNameSyntax.WithRight((SimpleNameSyntax)ApplyTypeParameters(qualifiedNameSyntax.Right));
                }

                return nameSyntax;
            }

            NameSyntax result = F.ParseName(symbol.ToMinimalDisplayString(_semanticModel, _classDeclaration.SpanStart));

            return ApplyTypeParameters(result);
        }

        public TypeSyntax ActionMethodMock()
        {
            return ParseTypeName(_mocklisSymbols.ActionMethodMock0);
        }

        public TypeSyntax ActionMethodMock(TypeSyntax tparam)
        {
            return ParseGenericName(_mocklisSymbols.ActionMethodMock1, tparam);
        }

        public TypeSyntax EventMock(TypeSyntax thandler)
        {
            return ParseGenericName(_mocklisSymbols.EventMock1, thandler);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.FuncMethodMock1, tresult);
        }

        public TypeSyntax FuncMethodMock(TypeSyntax tparam, TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.FuncMethodMock2, tparam, tresult);
        }

        public TypeSyntax IndexerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.IndexerMock2, tkey, tvalue);
        }

        public TypeSyntax PropertyMock(TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.PropertyMock1, tvalue);
        }

        public TypeSyntax Action(TypeSyntax t)
        {
            return ParseGenericName(_mocklisSymbols.Action1, t);
        }

        public TypeSyntax Action() => _action;

        public TypeSyntax ValueTuple => _valueTuple;

        public TypeSyntax EventStepCallerMock(TypeSyntax thandler)
        {
            return ParseGenericName(_mocklisSymbols.EventStepCaller1, thandler);
        }

        public TypeSyntax IndexerStepCallerMock(TypeSyntax tkey, TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.IndexerStepCaller2, tkey, tvalue);
        }

        public TypeSyntax MethodStepCallerMock(TypeSyntax tparam, TypeSyntax tresult)
        {
            return ParseGenericName(_mocklisSymbols.MethodStepCaller2, tparam, tresult);
        }

        public TypeSyntax PropertyStepCallerMock(TypeSyntax tvalue)
        {
            return ParseGenericName(_mocklisSymbols.PropertyStepCaller1, tvalue);
        }
    }
}
