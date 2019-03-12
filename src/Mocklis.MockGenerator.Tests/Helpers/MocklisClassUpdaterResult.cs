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

    #endregion

    public class MocklisClassUpdaterResult
    {
        public bool IsSuccess { get; }
        public string Code { get; }
        public string[] Errors { get; }

        private MocklisClassUpdaterResult(bool isSuccess, string code, string[] errors)
        {
            IsSuccess = isSuccess;
            Code = code;
            Errors = errors;
        }

        public static MocklisClassUpdaterResult Success(string code)
        {
            return new MocklisClassUpdaterResult(true, code, Array.Empty<string>());
        }

        public static MocklisClassUpdaterResult Failure(string code, string[] errors)
        {
            return new MocklisClassUpdaterResult(false, code, errors);
        }
    }
}
