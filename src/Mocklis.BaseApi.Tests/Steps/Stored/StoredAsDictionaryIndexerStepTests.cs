// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredAsDictionaryIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class StoredAsDictionaryIndexerStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IIndexers Sut => MockMembers;

        [Fact]
        public void BeBothResultAndOutParameter()
        {
            var step = MockMembers.Item.StoredAsDictionary(out var store);
            Assert.Same(step, store);
        }

        [Fact]
        public void ReturnStoredValues()
        {
            MockMembers.Item.StoredAsDictionary();

            Sut[1] = "Hello";
            Sut[2] = "World";

            Assert.Equal("Hello", Sut[1]);
            Assert.Equal("World", Sut[2]);
        }

        [Fact]
        public void ReturnDefaultForMissingValues()
        {
            var step = MockMembers.Item.StoredAsDictionary();
            Assert.Null(Sut[4]);
            Assert.Null(step[5]);
        }

        [Fact]
        public void AllowExternalModification()
        {
            MockMembers.Item.StoredAsDictionary(out var store);

            Sut[1] = "Hello";
            store[2] = "World";

            Assert.Equal("Hello", store[1]);
            Assert.Equal("World", Sut[2]);
        }

        [Fact]
        public void AllowAccessToUnderlyingDictionary()
        {
            MockMembers.Item.StoredAsDictionary(out var storedStep);

            Sut[1] = "Hello";
            string beforeClear = Sut[1];
            storedStep.Dictionary.Clear();
            string afterClear = Sut[1];

            Assert.Equal("Hello", beforeClear);
            Assert.Null(afterClear);
        }
    }
}
