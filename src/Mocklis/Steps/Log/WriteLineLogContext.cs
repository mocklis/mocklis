// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteLineLogContext.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Log context class that sends each written log line to an action. Contains a static instance that writes to the
    ///     console.
    ///     Implements the <see cref="ILogContext" /> interface.
    /// </summary>
    /// <seealso cref="ILogContext" />
    public class WriteLineLogContext : ILogContext
    {
        private readonly Action<string> _writeLine;

        /// <summary>
        ///     WriteLineLogContext singleton that writes log lines to the console.
        /// </summary>
        public static WriteLineLogContext Console { get; } = new WriteLineLogContext(System.Console.Out.WriteLine);

        /// <summary>
        ///     Initializes a new instance of the <see cref="WriteLineLogContext" /> class.
        /// </summary>
        /// <param name="writeLine">An action that will be taken whenever a log line is to be written.</param>
        public WriteLineLogContext(Action<string> writeLine)
        {
            _writeLine = writeLine ?? throw new ArgumentNullException(nameof(writeLine));
        }

        /// <summary>
        ///     Logs the fact that an event handler is being added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler.</param>
        public void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler? value) where THandler : Delegate
        {
            _writeLine($"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that an event handler has been added.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        public void LogAfterEventAdd(IMockInfo mockInfo)
        {
            _writeLine($"Done adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that adding an event handler threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogEventAddException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
        }

        /// <summary>
        ///     Logs the fact that an event handler is being removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler.</param>
        public void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler? value) where THandler : Delegate
        {
            _writeLine($"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that av event handler has been removed.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        public void LogAfterEventRemove(IMockInfo mockInfo)
        {
            _writeLine($"Done removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that removing an event handler threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogEventRemoveException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
        }

        /// <summary>
        ///     Logs the fact that an indexer is being read from.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="key">The indexer key used.</param>
        public void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key)
        {
            _writeLine($"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' using key '{key}'");
        }

        /// <summary>
        ///     Logs the fact that an indexer has been read from.
        /// </summary>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="value">The value that was read.</param>
        /// ram&gt;
        public void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine($"Received '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that reading from an indexer threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogIndexerGetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
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
            _writeLine(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}' using key '{key}'");
        }

        /// <summary>
        ///     Logs the fact that and indexer has been written to.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        public void LogAfterIndexerSet(IMockInfo mockInfo)
        {
            _writeLine($"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that writing to an indexer threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogIndexerSetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
        }

        /// <summary>
        ///     Logs the fact that a method is being called without parameters.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        public void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo)
        {
            _writeLine($"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that a method is being called without parameters.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters passed to the method.</param>
        public void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param)
        {
            _writeLine($"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with parameter: '{param}'");
        }

        /// <summary>
        ///     Logs the fact that a method has been called and didn't return a result.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        public void LogAfterMethodCallWithoutResult(IMockInfo mockInfo)
        {
            _writeLine($"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that a method has been called and returned a result.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="mockInfo">The mock information.</param>
        /// <param name="result">The result returned from the method.</param>
        public void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result)
        {
            _writeLine($"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with result: '{result}'");
        }

        /// <summary>
        ///     Logs the fact that an exception was thrown when calling a method.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogMethodCallException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Call to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
        }

        /// <summary>
        ///     Logs the fact that an property is being read from.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        public void LogBeforePropertyGet(IMockInfo mockInfo)
        {
            _writeLine($"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
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
            _writeLine($"Received '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that reading from an property threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is read.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogPropertyGetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
        }

        /// <summary>
        ///     Logs the fact that an property is being written to.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine($"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}'");
        }

        /// <summary>
        ///     Logs the fact that and property has been written to.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        public void LogAfterPropertySet(IMockInfo mockInfo)
        {
            _writeLine($"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'");
        }

        /// <summary>
        ///     Logs the fact that writing to an property threw an exception.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="exception">The exception that was thrown.</param>
        public void LogPropertySetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'");
        }
    }
}
