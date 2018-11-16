// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterGetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordAfterGetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStep<TKey, TValue, TRecord>
    {
        private readonly Func<object, TKey, TValue, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        public InstanceRecordAfterGetIndexerStep(Func<object, TKey, TValue, TRecord> selection, Func<object, Exception, TRecord> onError = null)
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
                    Add(_onError(instance, exception));
                }

                throw;
            }

            Add(_selection(instance, key, value));
            return value;
        }
    }
}
