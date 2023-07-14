// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IfSetPropertyStepTests.cs">
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

    public class IfSetPropertyStepTests
    {
        private IReadOnlyList<string> Gets { get; }
        private IReadOnlyList<string> Sets { get; }
        private IProperties Sut { get; }

        public IfSetPropertyStepTests()
        {
            var mockMembers = new MockMembers();

            IReadOnlyList<string>? gets = null;
            IReadOnlyList<string>? sets = null;

            mockMembers.StringProperty
                .IfSet(i => i
                    .RecordAfterGet(out gets)
                    .RecordBeforeSet(out sets)
                    .Join(i.ElseBranch));

            Gets = gets!;
            Sets = sets!;

            Sut = mockMembers;
        }

        [Fact]
        public void NotForwardGet()
        {
            var _ = Sut.StringProperty;
            Assert.Empty(Gets);
        }

        [Fact]
        public void ForwardSet()
        {
            Sut.StringProperty = "one";
            Assert.Single(Sets);
        }
    }
}
