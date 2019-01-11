// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordAfterGetPropertyStep<TValue, TRecord> : RecordPropertyStepBase<TValue, TRecord>
    {
        private readonly Func<TValue, TRecord> _selection;
        private readonly Func<Exception, TRecord> _onError;

        public RecordAfterGetPropertyStep(Func<TValue, TRecord> selection, Func<Exception, TRecord> onError = null)
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
                    Add(_onError(exception));
                }

                throw;
            }

            Add(_selection(value));
            return value;
        }
    }
}
