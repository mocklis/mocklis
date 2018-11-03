// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisIndexer.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using F = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    #endregion

    public class MocklisMethod : MocklisMember<IMethodSymbol>
    {
        public override TypeSyntax MockPropertyType { get; }

        private (string uniqueName, ParameterOrReturnValue item)[] Parameters { get; }

        public MocklisMethod(MocklisClass mocklisClass, INamedTypeSymbol interfaceSymbol, IMethodSymbol symbol) : base(mocklisClass,
            interfaceSymbol, symbol)
        {
            var parameterOrReturnValueList = new List<ParameterOrReturnValue>();

            if (!symbol.ReturnsVoid)
            {
                parameterOrReturnValueList.Add(new ParameterOrReturnValue(ParameterOrReturnValueKind.ReturnValue, "returnValue", symbol.ReturnType, mocklisClass.ParseTypeName(symbol.ReturnType)));
            }

            foreach (var parameter in symbol.Parameters)
            {
                if (parameter.IsThis)
                {
                    continue;
                }

                ParameterOrReturnValueKind kind;
                switch (parameter.RefKind)
                {
                    case RefKind.Ref:
                    {
                        kind = ParameterOrReturnValueKind.Ref;
                        
                        break;
                    }
                    case RefKind.Out:
                    {
                        kind = ParameterOrReturnValueKind.Out;
                        break;
                    }
                    case RefKind.In:
                    case RefKind.None:
                    {
                        kind = ParameterOrReturnValueKind.Normal;
                        break;
                    }
                    default:
                    {
                        continue;
                    }
                }
                parameterOrReturnValueList.Add(new ParameterOrReturnValue(kind, parameter.Name, parameter.Type, mocklisClass.ParseTypeName(parameter.Type)));
            }

            Parameters = Uniquifier.GetUniqueNames(parameterOrReturnValueList).ToArray();

            var parameterTypeSyntax = ParameterOrReturnValue.BuildType(Parameters.Where(i =>
                i.item.Kind == ParameterOrReturnValueKind.Normal || i.item.Kind == ParameterOrReturnValueKind.Ref));

            var returnValueTypeSyntax = ParameterOrReturnValue.BuildType(Parameters.Where(i =>
                    i.item.Kind == ParameterOrReturnValueKind.ReturnValue || i.item.Kind == ParameterOrReturnValueKind.Ref || i.item.Kind == ParameterOrReturnValueKind.Out));

            if (returnValueTypeSyntax == null)
            {
                MockPropertyType = parameterTypeSyntax == null
                    ? mocklisClass.ActionMethodMock()
                    : mocklisClass.ActionMethodMock(parameterTypeSyntax);
            }
            else
            {
                MockPropertyType = parameterTypeSyntax == null
                    ? mocklisClass.FuncMethodMock(returnValueTypeSyntax)
                    : mocklisClass.FuncMethodMock(parameterTypeSyntax, returnValueTypeSyntax);
            }
        }

        public override MemberDeclarationSyntax ExplicitInterfaceMember(string mockPropertyName)
        {
            return null;
        }

        private enum ParameterOrReturnValueKind
        {
            ReturnValue,
            Normal,
            Out,
            Ref
        }

        private class ParameterOrReturnValue : IHasPreferredName
        {
            public ParameterOrReturnValueKind Kind { get; }
            public string PreferredName { get; }
            public ITypeSymbol TypeSymbol { get; }
            public TypeSyntax TypeSyntax { get; }

            public ParameterOrReturnValue(ParameterOrReturnValueKind kind, string preferredName, ITypeSymbol typeSymbol, TypeSyntax typeSyntax)
            {
                Kind = kind;
                PreferredName = preferredName;
                TypeSymbol = typeSymbol;
                TypeSyntax = typeSyntax;
            }

            public static TypeSyntax BuildType(IEnumerable<(string uniqueName, ParameterOrReturnValue item)> items)
            {
                var itemsArray = items.ToArray();
                switch (itemsArray.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return itemsArray[0].item.TypeSyntax;
                    default:
                        return F.TupleType(F.SeparatedList(itemsArray.Select(a => F.TupleElement(a.item.TypeSyntax, F.Identifier(a.uniqueName)))));
                }
            }
        }
    }
}
