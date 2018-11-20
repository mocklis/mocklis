// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeRemoveEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class InstanceRecordBeforeRemoveEventStep<THandler, TRecord> : RecordEventStep<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<object, THandler, TRecord> _selection;

        public InstanceRecordBeforeRemoveEventStep(Func<object, THandler, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Remove(object instance, MemberMock memberMock, THandler value)
        {
            Add(_selection(instance, value));
            base.Remove(instance, memberMock, value);
        }
    }
}
