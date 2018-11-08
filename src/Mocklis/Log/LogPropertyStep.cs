// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Log
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
            _logContext = logContext;
        }

        public override TValue Get(object instance, MemberMock memberMock)
        {
            _logContext.LogBeforePropertyGet(memberMock);
            try
            {
                var result = base.Get(instance, memberMock);
                _logContext.LogAfterPropertyGet(memberMock, result);
                return result;
            }
            catch (Exception exception)
            {
                _logContext.LogPropertyGetException(memberMock, exception);
                throw;
            }
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            _logContext.LogBeforePropertySet(memberMock, value);
            try
            {
                base.Set(instance, memberMock, value);
                _logContext.LogAfterPropertySet(memberMock);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertySetException(memberMock, exception);
                throw;
            }
        }
    }
}
