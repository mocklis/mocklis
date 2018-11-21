// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeRemoveEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordBeforeRemoveEventStep<THandler, TRecord> : RecordEventStepBase<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<THandler, TRecord> _selection;

        public RecordBeforeRemoveEventStep(Func<THandler, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Remove(object instance, MemberMock memberMock, THandler value)
        {
            Add(_selection(value));
            base.Remove(instance, memberMock, value);
        }
    }
}
