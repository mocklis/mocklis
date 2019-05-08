// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock_this_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Mocklis.Core.Tests.Helpers;
    using Xunit;

    #endregion

    public class IndexerMock_this_should
    {
        private static FakeNextIndexerStep<TKey, TValue> NextStepFor<TKey, TValue>(ICanHaveNextIndexerStep<TKey, TValue> mock, TValue value)
        {
            return new FakeNextIndexerStep<TKey, TValue>(mock, value);
        }

        [Fact]
        public void send_mock_information_and_key_to_step_and_get_value_on_getting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(indexerMock, "5");

            string value = indexerMock[5];

            Assert.Equal(1, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
            Assert.Same(indexerMock, nextStep.LastGetMockInfo);
            Assert.Equal(5, nextStep.LastGetKey);
            Assert.Equal("5", value);
        }

        [Fact]
        public void return_default_if_no_step_in_lenient_mode_on_getting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            string result = indexerMock[5];
            Assert.Null(result);
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode_on_getting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5]);
            Assert.Equal(MockType.IndexerGet, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode_on_getting()
        {
            var indexerMock =
                new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5]);
            Assert.Equal(MockType.IndexerGet, ex.MemberType);
        }

        [Fact]
        public void return_default_if_cleared_in_lenient_mode_on_getting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            string result = indexerMock[5];
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
            Assert.Null(result);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode_on_getting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5]);
            Assert.Equal(MockType.IndexerGet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode_on_getting()
        {
            var indexerMock =
                new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5]);
            Assert.Equal(MockType.IndexerGet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void send_mock_information_key_and_value_to_step_on_setting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(indexerMock, "5");

            indexerMock[5] = "5";

            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(1, nextStep.SetCount);
            Assert.Same(indexerMock, nextStep.LastSetMockInfo);
            Assert.Equal(5, nextStep.LastSetKey);
            Assert.Equal("5", nextStep.LastSetValue);
        }


        [Fact]
        public void do_nothing_if_no_step_in_lenient_mode_on_setting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            indexerMock[5] = "5";
        }

        [Fact]
        public void throw_if_no_step_in_strict_mode_on_setting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5] = "5");
            Assert.Equal(MockType.IndexerSet, ex.MemberType);
        }

        [Fact]
        public void throw_if_no_step_in_very_strict_mode_on_setting()
        {
            var indexerMock =
                new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5] = "5");
            Assert.Equal(MockType.IndexerSet, ex.MemberType);
        }

        [Fact]
        public void do_nothing_if_cleared_in_lenient_mode_on_setting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            indexerMock[5] = "5";
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void throw_if_cleared_in_strict_mode_on_setting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5] = "5");
            Assert.Equal(MockType.IndexerSet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void throw_if_cleared_in_very_strict_mode_on_setting()
        {
            var indexerMock =
                new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5] = "5");
            Assert.Equal(MockType.IndexerSet, ex.MemberType);
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }
    }
}
