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
        private IReadOnlyList<int> Gets { get; }
        private IReadOnlyList<int> Sets { get; }
        private IIndexers Sut { get; }

        public IfSetIndexerStep_should()
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
        public void not_forward_Get()
        {
            var _ = Sut[1];
            Assert.Equal(0, Gets.Count);
        }

        [Fact]
        public void forward_Set()
        {
            Sut[1] = "one";
            Assert.Equal(1, Sets.Count);
        }
    }
}
