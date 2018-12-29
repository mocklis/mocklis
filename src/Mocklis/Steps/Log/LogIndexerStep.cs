// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogIndexerStep<TKey, TValue> : IndexerStepWithNext<TKey, TValue>
    {
        private readonly ILogContext _logContext;

        public LogIndexerStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public override TValue Get(IMockInfo mockInfo, TKey key)
        {
            _logContext.LogBeforeIndexerGet(mockInfo, key);
            TValue result;
            try
            {
                result = base.Get(mockInfo, key);
            }
            catch (Exception exception)
            {
                _logContext.LogIndexerGetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterIndexerGet(mockInfo, result);
            return result;
        }

        public override void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            _logContext.LogBeforeIndexerSet(mockInfo, key, value);
            try
            {
                base.Set(mockInfo, key, value);
            }
            catch (Exception exception)
            {
                _logContext.LogIndexerSetException(mockInfo, exception);
                throw;
            }

            _logContext.LogAfterIndexerSet(mockInfo);
        }
    }
}
