// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeCallMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<object, TParam, TRecord> _selection;

        public InstanceRecordBeforeCallMethodStep(Func<object, TParam, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            Add(_selection(mockInfo.MockInstance, param));
            return base.Call(mockInfo, param);
        }
    }
}
