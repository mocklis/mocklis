// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>, IFinalStep
    {
        private readonly TValue _value;

        public ReturnIndexerStep(TValue value)
        {
            _value = value;
        }

        public TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return _value;
        }

        public void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
        }
    }
}
