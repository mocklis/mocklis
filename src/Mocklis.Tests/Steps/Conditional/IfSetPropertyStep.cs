// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetPropertyStep.cs">
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

    public class IfSetPropertyStep_should
    {
        private IReadOnlyList<string> _gets;
        private IReadOnlyList<string> _sets;
        private IProperties Sut { get; }

        public IfSetPropertyStep_should()
        {
            var mockMembers = new MockMembers();

            mockMembers.Name
                .IfSet(i => i
                    .RecordAfterGet(out _gets, n => n)
                    .RecordBeforeSet(out _sets, n => n)
                    .Join(i.ElseBranch))
                .Dummy();

            Sut = mockMembers;
        }

        [Fact]
        public void not_forward_Get()
        {
            var _ = Sut.Name;
            Assert.Equal(0, _gets.Count);
        }

        [Fact]
        public void forward_Set()
        {
            Sut.Name = "one";
            Assert.Equal(1, _sets.Count);
        }
    }
}
