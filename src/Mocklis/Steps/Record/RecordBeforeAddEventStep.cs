// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordBeforeAddEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class RecordBeforeAddEventStep<THandler, TRecord> : RecordEventStepBase<THandler, TRecord> where THandler : Delegate
    {
        private readonly Func<THandler, TRecord> _selection;

        public RecordBeforeAddEventStep(Func<THandler, TRecord> selection)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            Add(_selection(value));
            base.Add(instance, memberMock, value);
        }
    }
}
