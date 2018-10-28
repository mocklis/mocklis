// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseMethodMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public abstract class BaseMethodMock<TParam, TResult> : MemberMock, IMethodMock<TParam, TResult>
    {
        public IMethodImplementation<TParam, TResult> Implementation { get; private set; } = MissingMethodImplementation<TParam, TResult>.Instance;

        protected internal BaseMethodMock(string interfaceName, string memberName, string memberMockName)
            : base(interfaceName, memberName, memberMockName)
        {
        }

        public IMethodMock<TParam, TResult> Use(IMethodImplementation<TParam, TResult> implementation)
        {
            Implementation = implementation ?? throw new ArgumentNullException(nameof(implementation));
            return this;
        }

        protected TResult Call(MemberMock d, TParam param)
        {
            return Implementation.Call(d, param);
        }
    }
}
