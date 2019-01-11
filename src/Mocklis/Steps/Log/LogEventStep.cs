// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogEventStep<THandler> : EventStepWithNext<THandler> where THandler : Delegate
    {
        private readonly ILogContext _logContext;

        public LogEventStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public override void Add(IMockInfo mockInfo, THandler value)
        {
            _logContext.LogBeforeEventAdd(mockInfo, value);
            try
            {
                base.Add(mockInfo, value);
            }
            catch (Exception exception)
            {
                _logContext.LogEventAddException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterEventAdd(mockInfo);
        }

        public override void Remove(IMockInfo mockInfo, THandler value)
        {
            _logContext.LogBeforeEventRemove(mockInfo, value);
            try
            {
                base.Remove(mockInfo, value);
            }
            catch (Exception exception)
            {
                _logContext.LogEventRemoveException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterEventRemove(mockInfo);
        }
    }
}
