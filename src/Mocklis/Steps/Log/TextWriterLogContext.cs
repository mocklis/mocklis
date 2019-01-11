// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterLogContext.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    #region Using Directives

    using System;
    using System.IO;
    using Mocklis.Core;

    #endregion

    public class TextWriterLogContext : ILogContext
    {
        public static readonly TextWriterLogContext Console = new TextWriterLogContext(System.Console.Out);

        private readonly TextWriter _textWriter;

        public TextWriterLogContext(TextWriter textWriter)
        {
            _textWriter = textWriter;
        }

        public void LogBeforeEventAdd<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterEventAdd(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogEventAddException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeEventRemove<THandler>(IMockInfo mockInfo, THandler value) where THandler : Delegate
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterEventRemove(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogEventRemoveException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Removing event handler from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerGet<TKey>(IMockInfo mockInfo, TKey key)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' using key {key}"));
        }

        public void LogAfterIndexerGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogIndexerGetException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerSet<TKey, TValue>(IMockInfo mockInfo, TKey key, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}' using key '{key}'"));
        }

        public void LogAfterIndexerSet(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogIndexerSetException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeMethodCallWithoutParameters(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant($"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogBeforeMethodCallWithParameters<TParam>(IMockInfo mockInfo, TParam param)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Calling '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with parameter: {param}"));
        }

        public void LogAfterMethodCallWithoutResult(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant($"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterMethodCallWithResult<TResult>(IMockInfo mockInfo, TResult result)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Returned from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' with result: {result}"));
        }

        public void LogMethodCallException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Call to '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertyGet(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogAfterPropertyGet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogPropertyGetException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Getting value from '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertySet<TValue>(IMockInfo mockInfo, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' to '{value}'"));
        }

        public void LogAfterPropertySet(IMockInfo mockInfo)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}'"));
        }

        public void LogPropertySetException(IMockInfo mockInfo, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Setting value on '[{mockInfo.MocklisClassName}] {mockInfo.InterfaceName}.{mockInfo.MemberName}' threw exception '{exception.Message}'"));
        }
    }
}
