// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceGetFuncPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class InstanceGetFuncPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void RequireNonNullAction()
        {
            Assert.Throws<ArgumentNullException>(() => MockMembers.StringProperty.InstanceGetFunc(null!));
        }

        [Fact]
        public void EvaluateFuncOnGets()
        {
            object? callInstance = null;
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
        public void ForwardSets()
        {
            MockMembers.StringProperty.InstanceGetFunc(o => throw new InvalidOperationException()).RecordBeforeSet(out var ledger);

            Sut.StringProperty = "Test";

            Assert.Equal(new[] { "Test" }, ledger);
        }
    }
}
