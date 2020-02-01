// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassUpdaterResult.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
            public int FirstLineNumber { get; }

            public Error(string errorText, string[] codeLines, int startPosition, int endPosition, int firstLineNumber)
            {
                ErrorText = errorText ?? throw new ArgumentNullException(nameof(errorText));
                CodeLines = codeLines ?? throw new ArgumentNullException(nameof(codeLines));
                StartPosition = startPosition;
                EndPosition = endPosition;
                FirstLineNumber = firstLineNumber;
            }

            public IEnumerable<string> MarkedCodeLines()
            {
                int count = CodeLines.Length;
                for (int i = 0; i < count; i++)
                {
                    string lineNumber = $"{i + FirstLineNumber,5}: ";
                    bool isFirst = i == 0;
                    bool isLast = i == count - 1;
                    string line = CodeLines[i];
                    yield return lineNumber + line;
                    int start = (isFirst ? StartPosition : 0) + lineNumber.Length;
                    int end = (isLast ? EndPosition : line.Length) + lineNumber.Length;

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
