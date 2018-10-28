// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteOnlyPropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class WriteOnlyPropertyMock<TValue> : MemberMock, IWriteOnlyPropertyMock<TValue>
    {
        public IPropertyImplementation<TValue> Implementation { get; private set; } = MissingPropertyImplementation<TValue>.Instance;

        public WriteOnlyPropertyMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public IWriteOnlyPropertyMock<TValue> Use(IPropertyImplementation<TValue> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        public void Set(TValue value)
        {
            Implementation.Set(this, value);
        }
    }
}
