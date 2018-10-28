// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyPropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class ReadOnlyPropertyMock<TValue> : MemberMock, IReadOnlyPropertyMock<TValue>
    {
        public IPropertyImplementation<TValue> Implementation { get; private set; } = MissingPropertyImplementation<TValue>.Instance;

        public ReadOnlyPropertyMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IReadOnlyPropertyMock<TValue> Use(IPropertyImplementation<TValue> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public TValue Get()
        {
            return Implementation.Get(this);
        }
    }
}
