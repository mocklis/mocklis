// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeSetIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        public override void Set(MemberMock memberMock, TKey key, TValue value)
        {
            Add(_selection(memberMock.MockInstance, key, value));
            base.Set(memberMock, key, value);
        }
    }
}
