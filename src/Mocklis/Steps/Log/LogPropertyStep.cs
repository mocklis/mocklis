// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogPropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private readonly ILogContext _logContext;

        public LogPropertyStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public override TValue Get(MemberMock memberMock)
        {
            _logContext.LogBeforePropertyGet(memberMock);
            TValue result;
            try
            {
                result = base.Get(memberMock);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertyGetException(memberMock, exception);
                throw;
            }

            _logContext.LogAfterPropertyGet(memberMock, result);
            return result;
        }

        public override void Set(MemberMock memberMock, TValue value)
        {
            _logContext.LogBeforePropertySet(memberMock, value);
            try
            {
                base.Set(memberMock, value);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertySetException(memberMock, exception);
                throw;
            }

            _logContext.LogAfterPropertySet(memberMock);
        }
    }
}
