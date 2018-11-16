// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceFuncIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceFuncIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>
    {
        private readonly Func<object, TKey, TValue> _func;

        public InstanceFuncIndexerStep(Func<object, TKey, TValue> func)
        {
            _func = func;
        }

        public TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return _func(instance, key);
        }

        public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
        }
    }
}
