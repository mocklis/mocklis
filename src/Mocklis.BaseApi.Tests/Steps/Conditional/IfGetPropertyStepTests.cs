// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfGetPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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

    public class IfGetPropertyStepTests
    {
        private IReadOnlyList<string> Gets { get; }
        private IReadOnlyList<string> Sets { get; }
        private IProperties Sut { get; }

        public IfGetPropertyStepTests()
        {
            var mockMembers = new MockMembers();

            IReadOnlyList<string>? gets = null;
            IReadOnlyList<string>? sets = null;

            mockMembers.StringProperty
                .IfGet(i => i
                    .RecordAfterGet(out gets)
                    .RecordBeforeSet(out sets)
                    .Join(i.ElseBranch));

            Gets = gets!;
            Sets = sets!;

            Sut = mockMembers;
        }

        [Fact]
        public void ForwardGet()
        {
            var _ = Sut.StringProperty;
            Assert.Equal(1, Gets.Count);
        }

        [Fact]
        public void NotForwardSet()
        {
            Sut.StringProperty = "one";
            Assert.Equal(0, Sets.Count);
        }
    }
}
