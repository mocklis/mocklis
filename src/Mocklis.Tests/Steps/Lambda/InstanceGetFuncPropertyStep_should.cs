// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceGetFuncPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class InstanceGetFuncPropertyStep_should
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.InstanceGetFunc(null));
        }

        [Fact]
        public void evaluate_func_on_gets()
        {
            object callInstance = null;
            MockMembers.StringProperty.InstanceGetFunc(obj =>
            {
                callInstance = obj;
                return "RESULT!!";
            });

            var result = Sut.StringProperty;

            Assert.Same(Sut, callInstance);
            Assert.Equal("RESULT!!", result);
        }

        [Fact]
        public void forward_sets()
        {
            MockMembers.StringProperty.InstanceGetFunc(o => throw new InvalidOperationException()).RecordBeforeSet(out var ledger);

            Sut.StringProperty = "Test";

            Assert.Equal(new[] { "Test" }, ledger);
        }
    }
}
