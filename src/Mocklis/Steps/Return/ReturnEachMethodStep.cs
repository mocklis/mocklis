// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    public class ReturnEachMethodStep<TParam, TResult> : MedialMethodStep<TParam, TResult>
    {
        private readonly object _lockobject = new object();
        private IEnumerator<TResult> _results;

        public ReturnEachMethodStep(IEnumerable<TResult> results)
        {
            _results = results?.GetEnumerator();
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            if (_results == null)
            {
                return base.Call(mockInfo, param);
            }

            lock (_lockobject)
            {
                if (_results != null)
                {
                    if (_results.MoveNext())
                    {
                        return _results.Current;
                    }

                    _results.Dispose();
                    _results = null;
                }
            }

            return base.Call(mockInfo, param);
        }
    }
}
