// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterLogContext.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

        public void LogBeforeEventAdd(MemberMock memberMock)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterEventAdd(MemberMock memberMock)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done adding event handler to '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogEventAddException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Adding event handler to '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeEventRemove(MemberMock memberMock)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Removing event handler from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterEventRemove(MemberMock memberMock)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done removing event handler from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogEventRemoveException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Removing event handler from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerGet<TKey>(MemberMock memberMock, TKey key)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Getting value from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' using key {key}"));
        }

        public void LogAfterIndexerGet<TValue>(MemberMock memberMock, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogIndexerGetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Getting value from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerSet<TKey, TValue>(MemberMock memberMock, TKey key, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Setting value on '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' to '{value}' using key '{key}'"));
        }

        public void LogAfterIndexerSet(MemberMock memberMock)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogIndexerSetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Setting value on '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeMethodCallWithoutParameters(MemberMock memberMock)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant($"Calling '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogBeforeMethodCallWithParameters<TParam>(MemberMock memberMock, TParam param)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Calling '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' with parameter: {param}"));
        }

        public void LogAfterMethodCallWithoutResult(MemberMock memberMock)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant($"Returned from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterMethodCallWithResult<TResult>(MemberMock memberMock, TResult result)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Returned from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' with result: {result}"));
        }

        public void LogMethodCallException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Call to '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertyGet(MemberMock memberMock)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Getting value from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterPropertyGet<TValue>(MemberMock memberMock, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Done getting value '{value}' from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogPropertyGetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Getting value from '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertySet<TValue>(MemberMock memberMock, TValue value)
        {
            _textWriter.WriteLine(FormattableString.Invariant(
                $"Setting value on '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' to '{value}'"));
        }

        public void LogAfterPropertySet(MemberMock memberMock)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Done setting value on '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogPropertySetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                FormattableString.Invariant(
                    $"Setting value on '[{memberMock.MocklisClassName}] {memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }
    }
}
