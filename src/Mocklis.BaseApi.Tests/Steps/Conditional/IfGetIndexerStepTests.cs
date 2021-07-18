// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetIndexerStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Conditional
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class IfGetIndexerStepTests
    {
        private IReadOnlyList<int> Gets { get; }
        private IReadOnlyList<int> Sets { get; }
        private IIndexers Sut { get; }

        public IfGetIndexerStepTests()
        {
            var mockMembers = new MockMembers();

            IReadOnlyList<int>? gets = null;
            IReadOnlyList<int>? sets = null;

            mockMembers.Item
                .IfGet(i => i
                    .RecordAfterGet(out gets, (a, _) => a)
                    .RecordBeforeSet(out sets, (a, _) => a)
                    .Join(i.ElseBranch));

            Gets = gets!;
            Sets = sets!;

            Sut = mockMembers;
        }

        [Fact]
        public void ForwardGet()
        {
            var _ = Sut[1];
            Assert.Equal(1, Gets.Count);
        }

        [Fact]
        public void NotForwardSet()
        {
            Sut[1] = "one";
            Assert.Equal(0, Sets.Count);
        }
    }
}
