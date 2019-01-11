// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeSetIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordBeforeSetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStepBase<TKey, TValue, TRecord>
    {
        private readonly Func<object, TKey, TValue, TRecord> _selection;

        public InstanceRecordBeforeSetIndexerStep(Func<object, TKey, TValue, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            Add(_selection(mockInfo.MockInstance, key, value));
            base.Set(mockInfo, key, value);
        }
    }
}
