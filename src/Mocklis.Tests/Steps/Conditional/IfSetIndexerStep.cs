// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetIndexerStep.cs">
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

    public class IfSetIndexerStep_should
    {
        private IReadOnlyList<int> _gets;
        private IReadOnlyList<int> _sets;
        private IIndexers Sut { get; }

        public IfSetIndexerStep_should()
        {
            var mockMembers = new MockMembers();

            mockMembers.Item
                .IfSet(i => i
                    .RecordAfterGet(out _gets, (a, _) => a)
                    .RecordBeforeSet(out _sets, (a, _) => a)
                    .Join(i.ElseBranch))
                .Dummy();

            Sut = mockMembers;
        }

        [Fact]
        public void not_forward_Get()
        {
            var _ = Sut[1];
            Assert.Equal(0, _gets.Count);
        }

        [Fact]
        public void forward_Set()
        {
            Sut[1] = "one";
            Assert.Equal(1, _sets.Count);
        }
    }
}
