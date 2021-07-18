// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class OnlySetIfChangedPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void OnlyForwardValuesIfChanged()
        {
            MockMembers.StringProperty.OnlySetIfChanged().RecordBeforeSet(out var ledger).Stored("");

            Sut.StringProperty = "Alpha";
            Sut.StringProperty = "Beta";
            Sut.StringProperty = "Beta";
            Sut.StringProperty = "Alpha";
            Sut.StringProperty = "Alpha";
            Sut.StringProperty = "Gamma";

            Assert.Equal(new[] { "Alpha", "Beta", "Alpha", "Gamma" }, ledger);
        }

        [Fact]
        public void OnlyForwardValuesIfComparerConsidersThemChanged()
        {
            MockMembers.IntProperty.OnlySetIfChanged(new ModuloNComparer(5)).RecordBeforeSet(out var ledger).Stored();

            Sut.IntProperty = 6;
            Sut.IntProperty = 11;
            Sut.IntProperty = 21;
            Sut.IntProperty = 20;
            Sut.IntProperty = 15;
            Sut.IntProperty = 2;

            Assert.Equal(new[] { 6, 20, 2 }, ledger);
        }

        [Fact]
        public void NotThrowIfNextStepMissing()
        {
            MockMembers.IntProperty.OnlySetIfChanged();
            Sut.IntProperty = 7;
        }
    }
}
