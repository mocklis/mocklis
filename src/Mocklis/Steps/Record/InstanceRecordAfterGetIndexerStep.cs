// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterGetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordAfterGetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStepBase<TKey, TValue, TRecord>
    {
        private readonly Func<object, TKey, TValue, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        public InstanceRecordAfterGetIndexerStep(Func<object, TKey, TValue, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
        }

        public override TValue Get(MemberMock memberMock, TKey key)
        {
            TValue value;
            try
            {
                value = base.Get(memberMock, key);
            }
            catch (Exception exception)
            {
                if (_onError != null)
                {
                    Add(_onError(memberMock.MockInstance, exception));
                }

                throw;
            }

            Add(_selection(memberMock.MockInstance, key, value));
            return value;
        }
    }
}
