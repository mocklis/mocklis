// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMockSetNextStepTests.cs">
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

    public class PropertyMockSetNextStepTests
    {
        private readonly PropertyMock<int> _propertyMock;

        public PropertyMockSetNextStepTests()
        {
            _propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName", Strictness.Lenient);
        }

        [Fact]
        public void RequireStep()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep((IPropertyStep<int>)null!));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void ReturnNewStep()
        {
            var newStep = new MockPropertyStep<int>();
            var returnedStep = ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void SetStepUsedByValueGetter()
        {
            bool called = false;
            var newStep = new MockPropertyStep<int>();
            newStep.Get.Func(_ =>
            {
                called = true;
                return 5;
            });
            ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep(newStep);
            // ReSharper disable once UnusedVariable
            var ignored = _propertyMock.Value;
            Assert.True(called);
        }

        [Fact]
        public void SetStepUsedByValueSetter()
        {
            bool called = false;
            var newStep = new MockPropertyStep<int>();
            newStep.Set.Action(_ => called = true);
            ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep(newStep);
            _propertyMock.Value = 5;
            Assert.True(called);
        }
    }
}
