// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassUpdaterResult.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.Tests.Helpers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    #endregion

    public sealed class MocklisClassUpdaterResult
    {
        public sealed class Error
        {
            public string ErrorText { get; }
            public string[] CodeLines { get; }
            public int StartPosition { get; }
            public int EndPosition { get; }

            public Error(string errorText, string[] codeLines, int startPosition, int endPosition)
            {
                ErrorText = errorText ?? throw new ArgumentNullException(nameof(errorText));
                CodeLines = codeLines ?? throw new ArgumentNullException(nameof(codeLines));
                StartPosition = startPosition;
                EndPosition = endPosition;
            }

            public IEnumerable<string> MarkedCodeLines()
            {
                int count = CodeLines.Length;
                for (int i = 0; i < count; i++)
                {
                    bool isFirst = i == 0;
                    bool isLast = i == count - 1;
                    string line = CodeLines[i];
                    yield return line;
                    int start = isFirst ? StartPosition : 0;
                    int end = isLast ? EndPosition : line.Length;
                    yield return new string(' ', start) + new string('^', end - start);
                }
            }
        }

        public bool IsSuccess { get; }
        public string Code { get; }
        public Error[] Errors { get; }

        private MocklisClassUpdaterResult(bool isSuccess, string code, Error[] errors)
        {
            IsSuccess = isSuccess;
            Code = code;
            Errors = errors;
        }

        public static MocklisClassUpdaterResult Success(string code)
        {
            return new MocklisClassUpdaterResult(true, code, Array.Empty<Error>());
        }

        public static MocklisClassUpdaterResult Failure(string code, Error[] errors)
        {
            return new MocklisClassUpdaterResult(false, code, errors);
        }
    }
}
