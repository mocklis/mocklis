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
        private IReadOnlyList<string> Gets { get; }
        private IReadOnlyList<string> Sets { get; }
        private IProperties Sut { get; }

        public IfSetPropertyStep_should()
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
        public void not_forward_Get()
        {
            var _ = Sut.StringProperty;
            Assert.Equal(0, Gets.Count);
        }

        [Fact]
        public void forward_Set()
        {
            Sut.StringProperty = "one";
            Assert.Equal(1, Sets.Count);
        }
    }
}
