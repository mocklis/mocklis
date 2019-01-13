// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypedMockProvider.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

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

                var keyString = "<" + string.Join(",", types.Select(k => k.Name)) + ">";

                var mock = factory(keyString);
                _mocks[types] = mock;
                return mock;
            }
        }
    }
}
