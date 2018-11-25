// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeCallMethodStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordBeforeCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<TParam, TRecord> _selection;

        public RecordBeforeCallMethodStep(Func<TParam, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            Add(_selection(param));
            return base.Call(mockInfo, param);
        }
    }
}
