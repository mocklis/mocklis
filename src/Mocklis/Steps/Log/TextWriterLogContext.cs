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

    public class WriteLineLogContext : ILogContext
    {
        private readonly Action<string> _writeLine;
        public static readonly WriteLineLogContext Console = new WriteLineLogContext(s => System.Console.Out.WriteLine(s));

        public WriteLineLogContext(Action<string> writeLine)
        {
            _writeLine = writeLine ?? throw new ArgumentNullException(nameof(writeLine));
        }

        public void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterEventAdd(IMockInfo mockInfo)
        {
            _writeLine(FormattableString.Invariant(
                $"Done adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogEventAddException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _writeLine(FormattableString.Invariant(
                $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterEventRemove(IMockInfo mockInfo)
        {
            _writeLine(FormattableString.Invariant(
                $"Done removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogEventRemoveException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key)
        {
            _writeLine(FormattableString.Invariant(
                $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' using key '{key}'"));
        }

        public void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogIndexerGetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}' using key '{key}'"));
        }

        public void LogAfterIndexerSet(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogIndexerSetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant($"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with parameter: '{param}'"));
        }

        public void LogAfterMethodCallWithoutResult(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant($"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with result: '{result}'"));
        }

        public void LogMethodCallException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Call to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertyGet(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogPropertyGetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _writeLine(FormattableString.Invariant(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}'"));
        }

        public void LogAfterPropertySet(IMockInfo mockInfo)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogPropertySetException(IMockInfo mockInfo, Exception exception)
        {
            _writeLine(
                FormattableString.Invariant(
                    $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }
    }
}
