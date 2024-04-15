// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchMethodStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Switch
{
    #region Using Directives

    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class SwitchMethodStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();

        [Fact]
        public void PicksTheRightEntries()
        {
            var sw = MockMembers.FuncWithParameter.Switch();
            sw.When(a => a == 5).Return(10);
            sw.When(a => a == 10).Return(20);

            var result1 = MockMembers.FuncWithParameter.Call(5);
            var result2 = MockMembers.FuncWithParameter.Call(10);

            Assert.Equal(10, result1);
            Assert.Equal(20, result2);
        }

        [Fact]
        public void PicksEntriesInOrderOfRegistration()
        {
            var sw = MockMembers.FuncWithParameter.Switch();
            sw.When(a => a == 5).Return(10);
            sw.When(a => a == 5).Return(20);

            var result = MockMembers.FuncWithParameter.Call(5);
            Assert.Equal(10, result);
        }

        [Fact]
        public void DefaultsToOtherwise()
        {
            var sw = MockMembers.FuncWithParameter.Switch();
            sw.When(a => a == 5).Return(10);
            sw.Otherwise.Return(20);

            var result = MockMembers.FuncWithParameter.Call(7);
            Assert.Equal(20, result);
        }

        [Fact]
        public void CanConsumeEntries()
        {
            var sw = MockMembers.FuncWithParameter.Switch(consumeOnUse: true);
            sw.When(a => a == 5).Return(10);
            sw.When(a => a == 5).Return(20);
            sw.Otherwise.Return(42);

            var result1 = MockMembers.FuncWithParameter.Call(5);
            var result2 = MockMembers.FuncWithParameter.Call(5);
            var result3 = MockMembers.FuncWithParameter.Call(5);

            Assert.Equal(10, result1);
            Assert.Equal(20, result2);
            Assert.Equal(42, result3);
        }

        [Fact]
        public void CanClear()
        {
            var sw = MockMembers.FuncWithParameter.Switch();
            sw.When(a => a == 5).Return(10);
            sw.Otherwise.Return(20);
            sw.Clear();

            var result = MockMembers.FuncWithParameter.Call(5);
            Assert.Equal(default, result);
        }
    }
}
