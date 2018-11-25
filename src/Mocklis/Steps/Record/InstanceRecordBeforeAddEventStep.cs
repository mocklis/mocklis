// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeAddEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordBeforeAddEventStep<THandler, TRecord> : RecordEventStepBase<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<object, THandler, TRecord> _selection;

        public InstanceRecordBeforeAddEventStep(Func<object, THandler, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Add(IMockInfo mockInfo, THandler value)
        {
            Add(_selection(mockInfo.MockInstance, value));
            base.Add(mockInfo, value);
        }
    }
}
