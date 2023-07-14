// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetIndexerStepTests.cs">
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

    public class IfSetIndexerStepTests
    {
        private IReadOnlyList<int> Gets { get; }
        private IReadOnlyList<int> Sets { get; }
        private IIndexers Sut { get; }

        public IfSetIndexerStepTests()
        {
            var mockMembers = new MockMembers();

            IReadOnlyList<int>? gets = null;
            IReadOnlyList<int>? sets = null;

            mockMembers.Item
                .IfSet(i => i
                    .RecordAfterGet(out gets, (a, _) => a)
                    .RecordBeforeSet(out sets, (a, _) => a)
                    .Join(i.ElseBranch));

            Gets = gets!;
            Sets = sets!;

            Sut = mockMembers;
        }

        [Fact]
        public void NotForwardGet()
        {
            var _ = Sut[1];
            Assert.Empty(Gets);
        }

        [Fact]
        public void ForwardSet()
        {
            Sut[1] = "one";
            Assert.Single(Sets);
        }
    }
}
