// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeSetPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordBeforeSetPropertyStep<TValue, TRecord> : RecordPropertyStep<TValue, TRecord>
    {
        private readonly Func<object, TValue, TRecord> _selection;

        public InstanceRecordBeforeSetPropertyStep(Func<object, TValue, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            Add(_selection(instance, value));
            base.Set(instance, memberMock, value);
        }
    }
}
