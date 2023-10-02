// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System.Linq;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class TimesMethodStepTests
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IMethods Sut => MockMembers;

        [Fact]
        public void SendFirstInvocationsToOneStepAndRestToAnother()
        {
            MockMembers.FuncWithParameter
                .Times(3, step => step.Func(a => a))
                .Func(a => 10 + a);

            var result = Enumerable.Range(1, 6).Select(a => Sut.FuncWithParameter(a));

            Assert.Equal(new[] { 1, 2, 3, 14, 15, 16 }, result);
        }
    }
}
