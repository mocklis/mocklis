// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterGetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            TValue value;
            try
            {
                value = base.Get(instance, memberMock, key);
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
