// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XUnitTestClass.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using Xunit.Abstractions;

    #endregion

    public abstract class XUnitTestClass
    {
        protected ITestOutputHelper TestOutputHelper { get; }

        protected XUnitTestClass(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        protected void Log(string logLine)
        {
            TestOutputHelper.WriteLine(logLine);
        }
    }
}
