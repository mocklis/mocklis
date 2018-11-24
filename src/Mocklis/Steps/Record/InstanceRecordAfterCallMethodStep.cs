// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterCallMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<object, TParam, TResult, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        public InstanceRecordAfterCallMethodStep(Func<object, TParam, TResult, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        public override TResult Call(MemberMock memberMock, TParam param)
        {
            TResult result;
            try
            {
                result = base.Call(memberMock, param);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(memberMock.MockInstance, exception));
                }

                throw;
            }

            Add(_selection(memberMock.MockInstance, param, result));

            return result;
        }
    }
}