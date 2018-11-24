// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock_Value_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class PropertyMock_Value_should
    {
        private readonly PropertyMock<int> _propertyMock;

        public PropertyMock_Value_should()
        {
            _propertyMock = new PropertyMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_instance_to_step_and_get_value_on_getting()
        {
            MemberMock sentInstance = null;

            var newStep = new MockPropertyStep<int>();
            newStep.Get.Func(i =>
            {
                sentInstance = i;
                return 5;
            });
            _propertyMock.SetNextStep(newStep);

            int value = _propertyMock.Value;
            Assert.Equal(5, value);
            Assert.Same(_propertyMock, sentInstance);
        }

        [Fact]
        public void send_mock_instance_and_value_to_step_on_setting()
        {
            MemberMock sentInstance = null;
            int newValue = 0;

            var newStep = new MockPropertyStep<int>();
            newStep.Set.Action(p =>
            {
                sentInstance = p.memberMock;
                newValue = p.value;
            });
            _propertyMock.SetNextStep(newStep);

            _propertyMock.Value = 5;

            Assert.Same(_propertyMock, sentInstance);
            Assert.Equal(5, newValue);
        }
    }
}
