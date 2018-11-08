// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterLogContext.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Log
{
    #region Using Directives

    using System;
    using System.IO;
    using Mocklis.Core;
    using static System.FormattableString;

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
            _textWriter.WriteLine(Invariant($"Adding event handler to '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterEventAdd(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Done adding event handler to '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogEventAddException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Adding event handler to '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeEventRemove(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Removing event handler from '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterEventRemove(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Done removing event handler from '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogEventRemoveException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Removing event handler from '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerGet<TKey>(MemberMock memberMock, TKey key)
        {
            _textWriter.WriteLine(Invariant($"Getting value from '{memberMock.InterfaceName}.{memberMock.MemberName}' using key {key}"));
        }

        public void LogAfterIndexerGet<TValue>(MemberMock memberMock, TValue value)
        {
            _textWriter.WriteLine(Invariant($"Done getting value '{value}' from '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogIndexerGetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Getting value from '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeIndexerSet<TKey, TValue>(MemberMock memberMock, TKey key, TValue value)
        {
            _textWriter.WriteLine(Invariant($"Setting value '{value}' on '{memberMock.InterfaceName}.{memberMock.MemberName}' using key '{key}'"));
        }

        public void LogAfterIndexerSet(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Done setting value on '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogIndexerSetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Setting value on '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforeMethodCallWithoutParameters(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Calling '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogBeforeMethodCallWithParameters<TParam>(MemberMock memberMock, TParam param)
        {
            _textWriter.WriteLine(
                Invariant($"Calling '{memberMock.InterfaceName}.{memberMock.MemberName}' with parameter: {param}"));
        }

        public void LogAfterMethodCallWithoutResult(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Returned from '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterMethodCallWithResult<TResult>(MemberMock memberMock, TResult result)
        {
            _textWriter.WriteLine(
                Invariant($"Returned from '{memberMock.InterfaceName}.{memberMock.MemberName}' with result: {result}"));
        }

        public void LogMethodCallException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Call to '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertyGet(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Getting value from '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterPropertyGet<TValue>(MemberMock memberMock, TValue value)
        {
            _textWriter.WriteLine(Invariant($"Done getting value '{value}' from '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogPropertyGetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Getting value from '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }

        public void LogBeforePropertySet<TValue>(MemberMock memberMock, TValue value)
        {
            _textWriter.WriteLine(Invariant($"Setting value '{value}' on '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogAfterPropertySet(MemberMock memberMock)
        {
            _textWriter.WriteLine(Invariant($"Done setting value on '{memberMock.InterfaceName}.{memberMock.MemberName}'"));
        }

        public void LogPropertySetException(MemberMock memberMock, Exception exception)
        {
            _textWriter.WriteLine(
                Invariant($"Setting value on '{memberMock.InterfaceName}.{memberMock.MemberName}' threw exception '{exception.Message}'"));
        }
    }
}
