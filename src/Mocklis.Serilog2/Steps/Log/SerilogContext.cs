// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerilogContext.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Serilog;
    using Serilog.Events;

    #endregion

    public class SerilogContext : ILogContext
    {
        private readonly ILogger _logger;
        private readonly LogEventLevel _normalLogLevel;
        private readonly LogEventLevel _errorLogLevel;

        public SerilogContext(ILogger logger, LogEventLevel? normalLogLevel = null, LogEventLevel? errorLogLevel = null)
        {
            _logger = logger;
            _normalLogLevel = normalLogLevel ?? LogEventLevel.Debug;
            _errorLogLevel = errorLogLevel ?? LogEventLevel.Error;
        }

        public void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _logger.Write(_normalLogLevel, "Adding event handler to [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogAfterEventAdd(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel, "Done adding event handler to [{MocklisClassName}] {InterfaceName}.{MemberName}",
                mockInfo.MocklisClassName, mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogEventAddException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Adding event handler to [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        public void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _logger.Write(_normalLogLevel,
                "Removing event handler from [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        public void LogAfterEventRemove(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Done removing event handler from [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogEventRemoveException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Removing event handler from [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}",
                mockInfo.MocklisClassName, mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        public void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key)
        {
            _logger.Write(_normalLogLevel,
                "Getting value from [{MocklisClassName}] {InterfaceName}.{MemberName} using key {@key}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, key);
        }

        public void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Done getting value {value} from [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogIndexerGetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Getting value from [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        public void LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Setting value on [{MocklisClassName}] {InterfaceName}.{MemberName} to {@value} using key {@key}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, value, key);
        }

        public void LogAfterIndexerSet(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Done setting value on [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        public void LogIndexerSetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Setting value on [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        public void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel, "Calling [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param)
        {
            _logger.Write(_normalLogLevel,
                "Calling [{MocklisClassName}] {InterfaceName}.{MemberName} with parameter: {@param}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, param);
        }

        public void LogAfterMethodCallWithoutResult(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel, "Returned from [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result)
        {
            _logger.Write(_normalLogLevel,
                "Returned from [{MocklisClassName}] {InterfaceName}.{MemberName} with result: {@result}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, result);
        }

        public void LogMethodCallException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Call to [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        public void LogBeforePropertyGet(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Getting value from [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        public void LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Done getting value {value} from [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        public void LogPropertyGetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Getting value from [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        public void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Setting value on [{MocklisClassName}] {InterfaceName}.{MemberName} to {@value}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, value);
        }

        public void LogAfterPropertySet(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Done setting value on [{MocklisClassName}] {InterfaceName}.{MemberName}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        public void LogPropertySetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Setting value on [{MocklisClassName}] {InterfaceName}.{MemberName} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }
    }
}
