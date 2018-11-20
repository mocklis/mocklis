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
        void LogBeforeEventAdd(MemberMock memberMock);
        void LogAfterEventAdd(MemberMock memberMock);
        void LogEventAddException(MemberMock memberMock, Exception exception);
        void LogBeforeEventRemove(MemberMock memberMock);
        void LogAfterEventRemove(MemberMock memberMock);

        void LogBeforeIndexerGet<TKey>(MemberMock memberMock, TKey key);
        void LogAfterIndexerGet<TValue>(MemberMock memberMock, TValue value);
        void LogIndexerGetException(MemberMock memberMock, Exception exception);
        void LogBeforeIndexerSet<TKey, TValue>(MemberMock memberMock, TKey key, TValue value);
        void LogAfterIndexerSet(MemberMock memberMock);
        void LogIndexerSetException(MemberMock memberMock, Exception exception);

        void LogBeforeMethodCallWithoutParameters(MemberMock memberMock);
        void LogBeforeMethodCallWithParameters<TParam>(MemberMock memberMock, TParam param);
        void LogAfterMethodCallWithoutResult(MemberMock memberMock);
        void LogAfterMethodCallWithResult<TResult>(MemberMock memberMock, TResult result);
        void LogMethodCallException(MemberMock memberMock, Exception exception);

        void LogEventRemoveException(MemberMock memberMock, Exception exception);
        void LogBeforePropertyGet(MemberMock memberMock);
        void LogAfterPropertyGet<TValue>(MemberMock memberMock, TValue value);
        void LogPropertyGetException(MemberMock memberMock, Exception exception);
        void LogBeforePropertySet<TValue>(MemberMock memberMock, TValue value);
        void LogAfterPropertySet(MemberMock memberMock);
        void LogPropertySetException(MemberMock memberMock, Exception exception);
    }
}
