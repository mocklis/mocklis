// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterCallMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord> : RecordMethodStep<TParam, TResult, TRecord>
    {
        private readonly Func<object, TParam, TResult, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        public InstanceRecordAfterCallMethodStep(Func<object, TParam, TResult, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        public override TResult Call(object instance, MemberMock memberMock, TParam param)
        {
            TResult result;
            try
            {
                result = base.Call(instance, memberMock, param);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(instance, exception));
                }

                throw;
            }

            Add(_selection(instance, param, result));

            return result;
        }
    }
}
