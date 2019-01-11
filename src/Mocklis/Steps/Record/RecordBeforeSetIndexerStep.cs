// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeSetIndexerStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordBeforeSetIndexerStep<TKey, TValue, TRecord> : RecordIndexerStepBase<TKey, TValue, TRecord>
    {
        private readonly Func<TKey, TValue, TRecord> _selection;

        public RecordBeforeSetIndexerStep(Func<TKey, TValue, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            Add(_selection(key, value));
            base.Set(mockInfo, key, value);
        }
    }
}
