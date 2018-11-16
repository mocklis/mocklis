// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeAddEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordBeforeAddEventStep<THandler, TRecord> : RecordEventStep<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<object, THandler, TRecord> _selection;

        public InstanceRecordBeforeAddEventStep(Func<object, THandler, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            Add(_selection(instance, value));
            base.Add(instance, memberMock, value);
        }
    }
}
