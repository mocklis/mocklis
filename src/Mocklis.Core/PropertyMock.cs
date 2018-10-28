// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class PropertyMock<TValue> : MemberMock, IPropertyMock<TValue>
    {
        public IPropertyImplementation<TValue> Implementation { get; private set; } = MissingPropertyImplementation<TValue>.Instance;

        public PropertyMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IPropertyMock<TValue> Use(IPropertyImplementation<TValue> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public TValue Get()
        {
            return Implementation.Get(this);
        }

        public void Set(TValue value)
        {
            Implementation.Set(this, value);
        }
    }
}
