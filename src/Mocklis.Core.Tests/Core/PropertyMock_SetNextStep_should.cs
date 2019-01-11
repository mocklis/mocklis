// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock_SetNextStep_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using System;
    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class PropertyMock_SetNextStep_should
    {
        private readonly PropertyMock<int> _propertyMock;

        public PropertyMock_SetNextStep_should()
        {
            _propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void require_step()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep((IPropertyStep<int>)null));
            Assert.Equal("step", exception.ParamName);
        }

        [Fact]
        public void return_new_step()
        {
            var newStep = new MockPropertyStep<int>();
            var returnedStep = ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep(newStep);
            Assert.Same(newStep, returnedStep);
        }

        [Fact]
        public void set_step_used_by_Value_getter()
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
        public void set_step_used_by_Value_setter()
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
