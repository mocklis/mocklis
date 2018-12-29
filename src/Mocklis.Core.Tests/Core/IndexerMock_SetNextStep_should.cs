// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock_SetNextStep_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class IndexerMock_SetNextStep_should
    {
        private readonly IndexerMock<int, string> _indexerMock;

        public IndexerMock_SetNextStep_should()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void require_step()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextIndexerStep<int, string>)_indexerMock).SetNextStep((IIndexerStep<int, string>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void return_new_step()
        {
            var newStep = new MockIndexerStep<int, string>();
            var returnedStep = ((ICanHaveNextIndexerStep<int, string>)_indexerMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact(DisplayName = "set step used by this[] getter")]
        public void set_step_used_by_thisX5BX5D_getter()
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

        [Fact(DisplayName = "set step used by this[] setter")]
        public void set_step_used_by_thisX5BX5D_setter()
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
