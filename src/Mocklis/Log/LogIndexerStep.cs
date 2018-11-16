// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public sealed class LogIndexerStep<TKey, TValue> : MedialIndexerStep<TKey, TValue>
    {
        private readonly ILogContext _logContext;

        public LogIndexerStep(ILogContext logContext)
        {
            _logContext = logContext ?? throw new ArgumentNullException(nameof(logContext));
        }

        public override TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            _logContext.LogBeforeIndexerGet(memberMock, key);
            TValue result;
            try
            {
                result = base.Get(instance, memberMock, key);
            }
            catch (Exception exception)
            {
                _logContext.LogIndexerGetException(memberMock, exception);
                throw;
            }

            _logContext.LogAfterIndexerGet(memberMock, result);
            return result;
        }

        public override void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            _logContext.LogBeforeIndexerSet(memberMock, key, value);
            try
            {
                base.Set(instance, memberMock, key, value);
            }
            catch (Exception exception)
            {
                _logContext.LogIndexerSetException(memberMock, exception);
                throw;
            }

            _logContext.LogAfterIndexerSet(memberMock);
        }
    }
}
