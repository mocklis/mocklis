// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordAfterGetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStepBase<TKey, TValue, TRecord>
    {
        private readonly Func<TKey, TValue, TRecord> _selection;
        private readonly Func<Exception, TRecord> _onError;

        public RecordAfterGetIndexerStep(Func<TKey, TValue, TRecord> selection, Func<Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            TValue value;
            try
            {
                value = base.Get(mockInfo, key);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(exception));
                }

                throw;
            }

            Add(_selection(key, value));
            return value;
        }
    }
}
