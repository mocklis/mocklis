// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterCallMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordAfterCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<TParam, TResult, TRecord> _selection;
        private readonly Func<Exception, TRecord> _onError;

        public RecordAfterCallMethodStep(Func<TParam, TResult, TRecord> selection, Func<Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            TResult result;
            try
            {
                result = base.Call(mockInfo, param);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(exception));
                }

                throw;
            }

            Add(_selection(param, result));

            return result;
        }
    }
}
