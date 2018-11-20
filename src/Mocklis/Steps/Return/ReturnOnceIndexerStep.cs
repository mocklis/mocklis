// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    public class ReturnOnceIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly TValue _value;
        private int _returnCount;

        public ReturnOnceIndexerStep(TValue value)
        {
            _value = value;
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _value;
            }

            return base.Get(instance, memberMock, key);
        }
    }
}
