// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnTaskStepExtensionTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Threading.Tasks;
    using Mocklis.Core;
    using Xunit;
    using Xunit.Abstractions;

    #endregion

    public class ReturnTaskStepExtensionTests
    {
        public ITestOutputHelper TestOutputHelper { get; }

        public ReturnTaskStepExtensionTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ReturnTaskRequiresCaller()
        {
            TestOutputHelper.WriteLine($"{nameof(ReturnTaskStepExtensions)}.{nameof(ReturnTaskStepExtensions.ReturnTask)} throws if caller is null.");

            Assert.Throws<ArgumentNullException>(() => ((ICanHaveNextMethodStep<int, Task>)null!).ReturnTask());
            Assert.Throws<ArgumentNullException>(() => ((ICanHaveNextMethodStep<int, Task<int>>)null!).ReturnTask());
        }

#if NETCOREAPP3_0
        [Fact]
        public void ReturnTaskRequiresCallerForValueTask()
        {
            TestOutputHelper.WriteLine(
                $"{nameof(ReturnTaskStepExtensions)}.{nameof(ReturnTaskStepExtensions.ReturnTask)} throws if caller is null. (ValueTask case)");

            Assert.Throws<ArgumentNullException>(() => ((ICanHaveNextMethodStep<int, ValueTask>)null!).ReturnTask());
            Assert.Throws<ArgumentNullException>(() => ((ICanHaveNextMethodStep<int, ValueTask<int>>)null!).ReturnTask());
        }

#endif
    }
}
