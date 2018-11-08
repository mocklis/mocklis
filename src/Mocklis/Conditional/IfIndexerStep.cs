// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Conditional
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class IfIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly Func<TKey, bool> _condition;
        private readonly MedialIndexerStep<TKey, TValue> _branch = new MedialIndexerStep<TKey, TValue>();

        public IfIndexerStep(Func<TKey, bool> condition, Action<IIndexerStepCaller<TKey, TValue>> branch)
        {
            _condition = condition;
            branch(_branch);
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return _condition(key) ? _branch.Get(instance, memberMock, key) : base.Get(instance, memberMock, key);
        }

        public override void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            if (_condition(key))
            {
                _branch.Set(instance, memberMock, key, value);
            }
            else
            {
                base.Set(instance, memberMock, key, value);
            }
        }
    }
}
