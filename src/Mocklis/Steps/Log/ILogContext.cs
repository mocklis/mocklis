// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogContext.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public interface ILogContext
    {
        void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate;
        void LogAfterEventAdd(IMockInfo mockInfo);
        void LogEventAddException(IMockInfo mockInfo, Exception exception);
        void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate;
        void LogAfterEventRemove(IMockInfo mockInfo);
        void LogEventRemoveException(IMockInfo mockInfo, Exception exception);

        void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key);
        void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value);
        void LogIndexerGetException(IMockInfo mockInfo, Exception exception);
        void LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value);
        void LogAfterIndexerSet(IMockInfo mockInfo);
        void LogIndexerSetException(IMockInfo mockInfo, Exception exception);

        void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo);
        void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param);
        void LogAfterMethodCallWithoutResult(IMockInfo mockInfo);
        void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result);
        void LogMethodCallException(IMockInfo mockInfo, Exception exception);

        void LogBeforePropertyGet(IMockInfo mockInfo);
        void LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value);
        void LogPropertyGetException(IMockInfo mockInfo, Exception exception);
        void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value);
        void LogAfterPropertySet(IMockInfo mockInfo);
        void LogPropertySetException(IMockInfo mockInfo, Exception exception);
    }
}
