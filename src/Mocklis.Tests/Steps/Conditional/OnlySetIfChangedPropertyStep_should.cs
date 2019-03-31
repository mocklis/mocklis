// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlySetIfChangedPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using Mocklis.Tests.Helpers;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class OnlySetIfChangedPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void only_forward_values_if_changed()
        {
            MockMembers.Name.OnlySetIfChanged().RecordBeforeSet(out var ledger, a => a).Stored();

            Sut.Name = "Alpha";
            Sut.Name = "Beta";
            Sut.Name = "Beta";
            Sut.Name = "Alpha";
            Sut.Name = "Alpha";
            Sut.Name = "Gamma";

            Assert.Equal(new[] { "Alpha", "Beta", "Alpha", "Gamma" }, ledger);
        }

        [Fact]
        public void only_forward_values_if_comparer_considers_them_changed()
        {
            MockMembers.Age.OnlySetIfChanged(new ModuloNComparer(5)).RecordBeforeSet(out var ledger, a => a).Stored();

            Sut.Age = 6;
            Sut.Age = 11;
            Sut.Age = 21;
            Sut.Age = 20;
            Sut.Age = 15;
            Sut.Age = 2;

            Assert.Equal(new[] { 6, 20, 2 }, ledger);
        }
    }
}
