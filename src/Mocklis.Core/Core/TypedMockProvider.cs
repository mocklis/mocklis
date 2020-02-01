// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedMockProvider.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    ///     'Dictionary' of mocks for the same mocked method with different type parameters. This class cannot be inherited.
    /// </summary>
    public sealed class TypedMockProvider
    {
        private sealed class TypeArrayComparer : IEqualityComparer<Type[]>
        {
            public static IEqualityComparer<Type[]> Instance { get; } = new TypeArrayComparer();

            private TypeArrayComparer()
            {
            }

            public bool Equals(Type[] left, Type[] right) => left == null ? right == null : right != null && left.SequenceEqual(right);

            public int GetHashCode(Type[] obj) => obj.Aggregate(0, (acc, t) => unchecked((acc * 23357) ^ t.GetHashCode()));
        }

        private readonly Dictionary<Type[], MemberMock> _mocks = new Dictionary<Type[], MemberMock>(TypeArrayComparer.Instance);

        private string GetNameOfType(Type type)
        {
            string name = type.Name;
            if (type.IsConstructedGenericType)
            {
                return name.Substring(0, name.IndexOf('`')) + TypeParameterString(type.GenericTypeArguments);
            }

            return name;
        }

        private string TypeParameterString(Type[] types)
        {
            return "<" + string.Join(",", types.Select(GetNameOfType)) + ">";
        }

        /// <summary>
        ///     Gets the existing mock for the given set of types, using the factory method to create one if it was not already
        ///     created.
        /// </summary>
        /// <param name="types">The types used as type parameters for the mocked method.</param>
        /// <param name="factory">A method that will be called to create the corresponding member mock.</param>
        /// <returns>The newly or previously created member mock for the given set of type parameters.</returns>
        public MemberMock GetOrAdd(Type[] types, Func<string, MemberMock> factory)
        {
            if (types == null)
            {
                throw new ArgumentNullException(nameof(types));
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            lock (_mocks)
            {
                if (_mocks.ContainsKey(types))
                {
                    return _mocks[types];
                }

                var mock = factory(TypeParameterString(types));
                _mocks[types] = mock;
                return mock;
            }
        }
    }
}
