// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeSetPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordBeforeSetPropertyStep<TValue, TRecord> : RecordPropertyStepBase<TValue, TRecord>
    {
        private readonly Func<TValue, TRecord> _selection;

        public RecordBeforeSetPropertyStep(Func<TValue, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Set(MemberMock memberMock, TValue value)
        {
            Add(_selection(value));
            base.Set(memberMock, value);
        }
    }
}
