// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterGetPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordAfterGetPropertyStep<TValue, TRecord> : RecordPropertyStepBase<TValue, TRecord>
    {
        private readonly Func<object, TValue, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        public InstanceRecordAfterGetPropertyStep(Func<object, TValue, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            TValue value;
            try
            {
                value = base.Get(mockInfo);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(mockInfo.MockInstance, exception));
                }

                throw;
            }

            Add(_selection(mockInfo.MockInstance, value));
            return value;
        }
    }
}
