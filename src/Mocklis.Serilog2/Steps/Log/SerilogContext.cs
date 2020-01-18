// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SerilogContext.cs">
//   SPDX-License-Identifier: MIT
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

    /// <summary>
    ///     Log context that writes to a Serilog 2.x logger.
    ///     Implements the <see cref="ILogContext" /> interface.
    /// </summary>
    /// <seealso cref="ILogContext" />
    public class SerilogContext : ILogContext
    {
        private readonly ILogger _logger;
        private readonly LogEventLevel _normalLogLevel;
        private readonly LogEventLevel _errorLogLevel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SerilogContext" /> class.
        /// </summary>
        /// <param name="logger">The Serilog <see cref="ILogger" /> to write logs to.</param>
        /// <param name="normalLogLevel">
        ///     The Serilog <see cref="LogEventLevel" /> to use for non-exceptional log events. Defaults
        ///     to Debug.
        /// </param>
        /// <param name="errorLogLevel">
        ///     The Serilog <see cref="LogEventLevel" /> to use for exceptional log events. Defaults to
        ///     Error.
        /// </param>
        public SerilogContext(ILogger logger, LogEventLevel? normalLogLevel = null, LogEventLevel? errorLogLevel = null)
        {
            _logger = logger;
            _normalLogLevel = normalLogLevel ?? LogEventLevel.Debug;
            _errorLogLevel = errorLogLevel ?? LogEventLevel.Error;
        }

        /// <summary>
        ///     Logs the fact that an event handler is being added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler.</param>
        public void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler? value) where THandler : Delegate
        {
            _logger.Write(_normalLogLevel, "Adding event handler to [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}",
                mockInfo.MocklisClassName, mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that an event handler has been added.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        public void LogAfterEventAdd(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel, "Done adding event handler to [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}",
                mockInfo.MocklisClassName, mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that adding an event handler threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogEventAddException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Adding event handler to [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}",
                mockInfo.MocklisClassName, mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        /// <summary>
        ///     Logs the fact that an event handler is being removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler.</param>
        public void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler? value) where THandler : Delegate
        {
            _logger.Write(_normalLogLevel,
                "Removing event handler from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that av event handler has been removed.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        public void LogAfterEventRemove(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Done removing event handler from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that removing an event handler threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogEventRemoveException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Removing event handler from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}",
                mockInfo.MocklisClassName, mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        /// <summary>
        ///     Logs the fact that an indexer is being read from.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        public void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key)
        {
            _logger.Write(_normalLogLevel,
                "Getting value from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} using key {key}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, key);
        }

        /// <summary>
        ///     Logs the fact that an indexer has been read from.
        /// </summary>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="value">The value that was read.</param>
        public void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Received {value} from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", value, mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that reading from an indexer threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogIndexerGetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Getting value from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        /// <summary>
        ///     Logs the fact that an indexer is being written to.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="key">The indexer key used.</param>
        /// <param name="value">The value being written.</param>
        public void LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Setting value on [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} to {value} using key {key}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, value, key);
        }

        /// <summary>
        ///     Logs the fact that and indexer has been written to.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        public void LogAfterIndexerSet(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Done setting value on [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that writing to an indexer threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogIndexerSetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Setting value on [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        /// <summary>
        ///     Logs the fact that a method is being called without parameters.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        public void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel, "Calling [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that a method is being called without parameters.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters passed to the method.</param>
        public void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param)
        {
            _logger.Write(_normalLogLevel,
                "Calling [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} with parameter: {param}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, param);
        }

        /// <summary>
        ///     Logs the fact that a method has been called and didn't return a result.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        public void LogAfterMethodCallWithoutResult(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel, "Returned from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that a method has been called and returned a result.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="mockInfo">The mock information.</param>
        /// <param name="result">The result returned from the method.</param>
        public void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result)
        {
            _logger.Write(_normalLogLevel,
                "Returned from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} with result: {result}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, result);
        }

        /// <summary>
        ///     Logs the fact that an exception was thrown when calling a method.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogMethodCallException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Call to [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        /// <summary>
        ///     Logs the fact that an property is being read from.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        public void LogBeforePropertyGet(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Getting value from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that an property has been read from.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="value">The value that was read.</param>
        /// ram&gt;
        public void LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Received {value} from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", value, mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that reading from an property threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogPropertyGetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Getting value from [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }

        /// <summary>
        ///     Logs the fact that an property is being written to.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _logger.Write(_normalLogLevel,
                "Setting value on [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} to {value}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, value);
        }

        /// <summary>
        ///     Logs the fact that and property has been written to.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        public void LogAfterPropertySet(IMockInfo mockInfo)
        {
            _logger.Write(_normalLogLevel,
                "Done setting value on [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l}", mockInfo.MocklisClassName, mockInfo.InterfaceName,
                mockInfo.MemberName);
        }

        /// <summary>
        ///     Logs the fact that writing to an property threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogPropertySetException(IMockInfo mockInfo, Exception exception)
        {
            _logger.Write(_errorLogLevel, exception,
                "Setting value on [{MocklisClassName:l}] {InterfaceName:l}.{MemberName:l} threw exception {Message}", mockInfo.MocklisClassName,
                mockInfo.InterfaceName, mockInfo.MemberName, exception.Message);
        }
    }
}
