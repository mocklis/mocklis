// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class IfGetPropertyStep_should
    {
        private IReadOnlyList<string> _gets;
        private IReadOnlyList<string> _sets;
        private IProperties Sut { get; }

        public IfGetPropertyStep_should()
        {
            var mockMembers = new MockMembers();

            mockMembers.StringProperty
                .IfGet(i => i
                    .RecordAfterGet(out _gets, n => n)
                    .RecordBeforeSet(out _sets, n => n)
                    .Join(i.ElseBranch))
                .Dummy();

            Sut = mockMembers;
        }

        [Fact]
        public void forward_Get()
        {
            var _ = Sut.StringProperty;
            Assert.Equal(1, _gets.Count);
        }

        [Fact]
        public void not_forward_Set()
        {
            Sut.StringProperty = "one";
            Assert.Equal(0, _sets.Count);
        }
    }
}
