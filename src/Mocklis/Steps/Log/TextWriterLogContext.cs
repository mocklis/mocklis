// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterLogContext.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
        public static readonly WriteLineLogContext Console = new WriteLineLogContext(s => System.Console.Out.WriteLine(s));

        /// <summary>
        ///     Initializes a new instance of the <see cref="WriteLineLogContext" /> class.
        /// </summary>
        /// <param name="writeLine">An action that will be taken whenever a log line is to be written.</param>
        public WriteLineLogContext(Action<string> writeLine)
        {
            _writeLine = writeLine ?? throw new ArgumentNullException(nameof(writeLine));
        }

        /// <inheritdoc />
        public void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogAfterEventAdd(IMockInfo mockInfo)
        {
            _writeLine(FormattableString.Invariant(
                $"Done adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogEventAddException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        /// <inheritdoc />
        public void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _writeLine(FormattableString.Invariant(
                $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogAfterEventRemove(IMockInfo mockInfo)
        {
            _writeLine(FormattableString.Invariant(
                $"Done removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogEventRemoveException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        /// <inheritdoc />
        public void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key)
        {
            _writeLine(FormattableString.Invariant(
                $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' using key '{key}'"));
        }

        /// <inheritdoc />
        public void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogIndexerGetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        /// <inheritdoc />
        public void LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}' using key '{key}'"));
        }

        /// <inheritdoc />
        public void LogAfterIndexerSet(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogIndexerSetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        /// <inheritdoc />
        public void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant($"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with parameter: '{param}'"));
        }

        /// <inheritdoc />
        public void LogAfterMethodCallWithoutResult(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant($"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with result: '{result}'"));
        }

        /// <inheritdoc />
        public void LogMethodCallException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Call to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        /// <inheritdoc />
        public void LogBeforePropertyGet(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogPropertyGetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        /// <inheritdoc />
        public void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}'"));
        }

        /// <inheritdoc />
        public void LogAfterPropertySet(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        /// <inheritdoc />
        public void LogPropertySetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }
    }
}
