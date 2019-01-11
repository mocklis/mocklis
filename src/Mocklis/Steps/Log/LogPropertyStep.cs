// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly ILogContext _logContext;

        public LogPropertyStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            _logContext.LogBeforePropertyGet(mockInfo);
            TValue result;
            try
            {
                result = base.Get(mockInfo);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertyGetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterPropertyGet(mockInfo, result);
            return result;
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            _logContext.LogBeforePropertySet(mockInfo, value);
            try
            {
                base.Set(mockInfo, value);
            }
            catch (Exception exception)
            {
                _logContext.LogPropertySetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterPropertySet(mockInfo);
        }
    }
}
