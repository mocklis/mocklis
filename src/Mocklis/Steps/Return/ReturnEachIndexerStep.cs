// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    public class ReturnEachIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly object _lockObject = new object();
        private IEnumerator<TValue> _values;

        public ReturnEachIndexerStep(IEnumerable<TValue> values)
        {
            _values = values?.GetEnumerator();
        }

        public override TValue Get(MemberMock memberMock, TKey key)
        {
            if (_values == null)
            {
                return base.Get(memberMock, key);
            }

            lock (_lockObject)
            {
                if (_values != null)
                {
                    if (_values.MoveNext())
                    {
                        return _values.Current;
                    }

                    _values.Dispose();
                    _values = null;
                }
            }

            return base.Get(memberMock, key);
        }
    }
}
