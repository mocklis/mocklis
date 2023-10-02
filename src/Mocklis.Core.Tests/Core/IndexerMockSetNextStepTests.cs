// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMockSetNextStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class IndexerMockSetNextStepTests
    {
        private readonly IndexerMock<int, string> _indexerMock;

        public IndexerMockSetNextStepTests()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
        }

        [Fact]
        public void RequireStep()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextIndexerStep<int, string>)_indexerMock).SetNextStep((IIndexerStep<int, string>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void ReturnNewStep()
        {
            var newStep = new MockIndexerStep<int, string>();
            var returnedStep = ((ICanHaveNextIndexerStep<int, string>)_indexerMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void SetStepUsedByGetter()
        {
            bool called = false;
            var newStep = new MockIndexerStep<int, string>();
            newStep.Get.Func(_ =>
            {
                called = true;
                return "5";
            });
            ((ICanHaveNextIndexerStep<int, string>)_indexerMock).SetNextStep(newStep);
            // ReSharper disable once UnusedVariable
            var ignored = _indexerMock[5];
            Assert.True(called);
        }

        [Fact]
        public void SetStepUsedBySetter()
        {
            bool called = false;
            var newStep = new MockIndexerStep<int, string>();
            newStep.Set.Action(_ => called = true);
            ((ICanHaveNextIndexerStep<int, string>)_indexerMock).SetNextStep(newStep);
            _indexerMock[5] = "5";
            Assert.True(called);
        }
    }
}
