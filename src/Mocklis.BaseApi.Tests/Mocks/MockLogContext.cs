// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockLogContext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System;
    using System.CodeDom.Compiler;
    using Mocklis.Core;
    using Mocklis.Steps.Log;

    #endregion

    [MocklisClass, GeneratedCode("Mocklis", "1.2.0")]
    public class MockLogContext : ILogContext
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockLogContext()
        {
            LogAfterEventAdd = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogAfterEventAdd", "LogAfterEventAdd", Strictness.Lenient);
            LogEventAddException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogEventAddException", "LogEventAddException", Strictness.Lenient);
            LogAfterEventRemove = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogAfterEventRemove", "LogAfterEventRemove", Strictness.Lenient);
            LogEventRemoveException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogEventRemoveException", "LogEventRemoveException", Strictness.Lenient);
            LogIndexerGetException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogIndexerGetException", "LogIndexerGetException", Strictness.Lenient);
            LogAfterIndexerSet = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogAfterIndexerSet", "LogAfterIndexerSet", Strictness.Lenient);
            LogIndexerSetException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogIndexerSetException", "LogIndexerSetException", Strictness.Lenient);
            LogBeforeMethodCallWithoutParameters = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogBeforeMethodCallWithoutParameters", "LogBeforeMethodCallWithoutParameters", Strictness.Lenient);
            LogAfterMethodCallWithoutResult = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogAfterMethodCallWithoutResult", "LogAfterMethodCallWithoutResult", Strictness.Lenient);
            LogMethodCallException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogMethodCallException", "LogMethodCallException", Strictness.Lenient);
            LogBeforePropertyGet = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogBeforePropertyGet", "LogBeforePropertyGet", Strictness.Lenient);
            LogPropertyGetException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogPropertyGetException", "LogPropertyGetException", Strictness.Lenient);
            LogAfterPropertySet = new ActionMethodMock<IMockInfo>(this, "MockLogContext", "ILogContext", "LogAfterPropertySet", "LogAfterPropertySet", Strictness.Lenient);
            LogPropertySetException = new ActionMethodMock<(IMockInfo mockInfo, Exception exception)>(this, "MockLogContext", "ILogContext", "LogPropertySetException", "LogPropertySetException", Strictness.Lenient);
        }

        private readonly TypedMockProvider _logBeforeEventAdd = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, THandler? value)> LogBeforeEventAdd<THandler>() where THandler : Delegate
        {
            var key = new[] { typeof(THandler) };
            return (ActionMethodMock<(IMockInfo mockInfo, THandler? value)>)_logBeforeEventAdd.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, THandler? value)>(this, "MockLogContext", "ILogContext", "LogBeforeEventAdd" + keyString, "LogBeforeEventAdd" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler? value) where THandler : class => LogBeforeEventAdd<THandler>().Call((mockInfo, value));

        public ActionMethodMock<IMockInfo> LogAfterEventAdd { get; }

        void ILogContext.LogAfterEventAdd(IMockInfo mockInfo) => LogAfterEventAdd.Call(mockInfo);

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogEventAddException { get; }

        void ILogContext.LogEventAddException(IMockInfo mockInfo, Exception exception) => LogEventAddException.Call((mockInfo, exception));

        private readonly TypedMockProvider _logBeforeEventRemove = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, THandler? value)> LogBeforeEventRemove<THandler>() where THandler : Delegate
        {
            var key = new[] { typeof(THandler) };
            return (ActionMethodMock<(IMockInfo mockInfo, THandler? value)>)_logBeforeEventRemove.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, THandler? value)>(this, "MockLogContext", "ILogContext", "LogBeforeEventRemove" + keyString, "LogBeforeEventRemove" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler? value) where THandler : class => LogBeforeEventRemove<THandler>().Call((mockInfo, value));

        public ActionMethodMock<IMockInfo> LogAfterEventRemove { get; }

        void ILogContext.LogAfterEventRemove(IMockInfo mockInfo) => LogAfterEventRemove.Call(mockInfo);

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogEventRemoveException { get; }

        void ILogContext.LogEventRemoveException(IMockInfo mockInfo, Exception exception) => LogEventRemoveException.Call((mockInfo, exception));

        private readonly TypedMockProvider _logBeforeIndexerGet = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TKey key)> LogBeforeIndexerGet<TKey>()
        {
            var key = new[] { typeof(TKey) };
            return (ActionMethodMock<(IMockInfo mockInfo, TKey key)>)_logBeforeIndexerGet.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TKey key)>(this, "MockLogContext", "ILogContext", "LogBeforeIndexerGet" + keyString, "LogBeforeIndexerGet" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key) => LogBeforeIndexerGet<TKey>().Call((mockInfo, key));

        private readonly TypedMockProvider _logAfterIndexerGet = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TValue value)> LogAfterIndexerGet<TValue>()
        {
            var key = new[] { typeof(TValue) };
            return (ActionMethodMock<(IMockInfo mockInfo, TValue value)>)_logAfterIndexerGet.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TValue value)>(this, "MockLogContext", "ILogContext", "LogAfterIndexerGet" + keyString, "LogAfterIndexerGet" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value) => LogAfterIndexerGet<TValue>().Call((mockInfo, value));

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogIndexerGetException { get; }

        void ILogContext.LogIndexerGetException(IMockInfo mockInfo, Exception exception) => LogIndexerGetException.Call((mockInfo, exception));

        private readonly TypedMockProvider _logBeforeIndexerSet = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)> LogBeforeIndexerSet<TKey, TValue>()
        {
            var key = new[] { typeof(TKey), typeof(TValue) };
            return (ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)>)_logBeforeIndexerSet.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TKey key, TValue value)>(this, "MockLogContext", "ILogContext", "LogBeforeIndexerSet" + keyString, "LogBeforeIndexerSet" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value) => LogBeforeIndexerSet<TKey, TValue>().Call((mockInfo, key, value));

        public ActionMethodMock<IMockInfo> LogAfterIndexerSet { get; }

        void ILogContext.LogAfterIndexerSet(IMockInfo mockInfo) => LogAfterIndexerSet.Call(mockInfo);

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogIndexerSetException { get; }

        void ILogContext.LogIndexerSetException(IMockInfo mockInfo, Exception exception) => LogIndexerSetException.Call((mockInfo, exception));

        public ActionMethodMock<IMockInfo> LogBeforeMethodCallWithoutParameters { get; }

        void ILogContext.LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo) => LogBeforeMethodCallWithoutParameters.Call(mockInfo);

        private readonly TypedMockProvider _logBeforeMethodCallWithParameters = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TParam param)> LogBeforeMethodCallWithParameters<TParam>()
        {
            var key = new[] { typeof(TParam) };
            return (ActionMethodMock<(IMockInfo mockInfo, TParam param)>)_logBeforeMethodCallWithParameters.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TParam param)>(this, "MockLogContext", "ILogContext", "LogBeforeMethodCallWithParameters" + keyString, "LogBeforeMethodCallWithParameters" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param) => LogBeforeMethodCallWithParameters<TParam>().Call((mockInfo, param));

        public ActionMethodMock<IMockInfo> LogAfterMethodCallWithoutResult { get; }

        void ILogContext.LogAfterMethodCallWithoutResult(IMockInfo mockInfo) => LogAfterMethodCallWithoutResult.Call(mockInfo);

        private readonly TypedMockProvider _logAfterMethodCallWithResult = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TResult result)> LogAfterMethodCallWithResult<TResult>()
        {
            var key = new[] { typeof(TResult) };
            return (ActionMethodMock<(IMockInfo mockInfo, TResult result)>)_logAfterMethodCallWithResult.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TResult result)>(this, "MockLogContext", "ILogContext", "LogAfterMethodCallWithResult" + keyString, "LogAfterMethodCallWithResult" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result) => LogAfterMethodCallWithResult<TResult>().Call((mockInfo, result));

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogMethodCallException { get; }

        void ILogContext.LogMethodCallException(IMockInfo mockInfo, Exception exception) => LogMethodCallException.Call((mockInfo, exception));

        public ActionMethodMock<IMockInfo> LogBeforePropertyGet { get; }

        void ILogContext.LogBeforePropertyGet(IMockInfo mockInfo) => LogBeforePropertyGet.Call(mockInfo);

        private readonly TypedMockProvider _logAfterPropertyGet = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TValue value)> LogAfterPropertyGet<TValue>()
        {
            var key = new[] { typeof(TValue) };
            return (ActionMethodMock<(IMockInfo mockInfo, TValue value)>)_logAfterPropertyGet.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TValue value)>(this, "MockLogContext", "ILogContext", "LogAfterPropertyGet" + keyString, "LogAfterPropertyGet" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value) => LogAfterPropertyGet<TValue>().Call((mockInfo, value));

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogPropertyGetException { get; }

        void ILogContext.LogPropertyGetException(IMockInfo mockInfo, Exception exception) => LogPropertyGetException.Call((mockInfo, exception));

        private readonly TypedMockProvider _logBeforePropertySet = new TypedMockProvider();

        public ActionMethodMock<(IMockInfo mockInfo, TValue value)> LogBeforePropertySet<TValue>()
        {
            var key = new[] { typeof(TValue) };
            return (ActionMethodMock<(IMockInfo mockInfo, TValue value)>)_logBeforePropertySet.GetOrAdd(key, keyString => new ActionMethodMock<(IMockInfo mockInfo, TValue value)>(this, "MockLogContext", "ILogContext", "LogBeforePropertySet" + keyString, "LogBeforePropertySet" + keyString + "()", Strictness.Lenient));
        }

        void ILogContext.LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value) => LogBeforePropertySet<TValue>().Call((mockInfo, value));

        public ActionMethodMock<IMockInfo> LogAfterPropertySet { get; }

        void ILogContext.LogAfterPropertySet(IMockInfo mockInfo) => LogAfterPropertySet.Call(mockInfo);

        public ActionMethodMock<(IMockInfo mockInfo, Exception exception)> LogPropertySetException { get; }

        void ILogContext.LogPropertySetException(IMockInfo mockInfo, Exception exception) => LogPropertySetException.Call((mockInfo, exception));
    }
}
