// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMockThisTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using Mocklis.Helpers;
    using Xunit;

    #endregion

    public class IndexerMockThisTests
    {
        private static FakeNextIndexerStep<TKey, TValue> NextStepFor<TKey, TValue>(ICanHaveNextIndexerStep<TKey, TValue> mock, TValue value)
        {
            return new FakeNextIndexerStep<TKey, TValue>(mock, value);
        }

        [Fact]
        public void SendMockInformationAndKeyToStepAndGetValueOnGetting()
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
        public void ReturnDefaultIfNoStepInLenientModeOnGetting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            string result = indexerMock[5];
            Assert.Null(result);
        }

        [Fact]
        public void ThrowIfNoStepInStrictModeOnGetting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5]);
            Assert.Equal(MockType.IndexerGet, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictModeOnGetting()
        {
            var indexerMock =
                new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5]);
            Assert.Equal(MockType.IndexerGet, ex.MemberType);
        }

        [Fact]
        public void ReturnDefaultIfClearedInLenientModeOnGetting()
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
        public void ThrowIfClearedInStrictModeOnGetting()
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
        public void ThrowIfClearedInVeryStrictModeOnGetting()
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
        public void SendMockInformationKeyAndValueToStepOnSetting()
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
        public void DoNothingIfNoStepInLenientModeOnSetting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            indexerMock[5] = "5";
        }

        [Fact]
        public void ThrowIfNoStepInStrictModeOnSetting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Strict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5] = "5");
            Assert.Equal(MockType.IndexerSet, ex.MemberType);
        }

        [Fact]
        public void ThrowIfNoStepInVeryStrictModeOnSetting()
        {
            var indexerMock =
                new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.VeryStrict);
            var ex = Assert.Throws<MockMissingException>(() => indexerMock[5] = "5");
            Assert.Equal(MockType.IndexerSet, ex.MemberType);
        }

        [Fact]
        public void DoNothingIfClearedInLenientModeOnSetting()
        {
            var indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
            var nextStep = NextStepFor(indexerMock, "5");
            indexerMock.Clear();
            indexerMock[5] = "5";
            Assert.Equal(0, nextStep.GetCount);
            Assert.Equal(0, nextStep.SetCount);
        }

        [Fact]
        public void ThrowIfClearedInStrictModeOnSetting()
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
        public void ThrowIfClearedInVeryStrictModeOnSetting()
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
