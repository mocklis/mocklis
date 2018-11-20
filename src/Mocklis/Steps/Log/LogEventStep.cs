// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogEventStep<THandler> : MedialEventStep<THandler> where THandler : Delegate
    {
        private readonly ILogContext _logContext;

        public LogEventStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public override void Add(object instance, MemberMock memberMock, THandler value)
        {
            _logContext.LogBeforeEventAdd(memberMock);
            try
            {
                base.Add(instance, memberMock, value);
            }
            catch (Exception exception)
            {
                _logContext.LogEventAddException(memberMock, exception);
                throw;
            }

            _logContext.LogAfterEventAdd(memberMock);
        }

        public override void Remove(object instance, MemberMock memberMock, THandler value)
        {
            _logContext.LogBeforeEventRemove(memberMock);
            try
            {
                base.Remove(instance, memberMock, value);
            }
            catch (Exception exception)
            {
                _logContext.LogEventRemoveException(memberMock, exception);
                throw;
            }

            _logContext.LogAfterEventRemove(memberMock);
        }
    }
}
