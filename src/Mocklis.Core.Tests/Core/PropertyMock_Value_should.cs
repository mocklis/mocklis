// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock_Value_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
        public void send_mock_information_to_step_and_get_value_on_getting()
        {
            IMockInfo sentMockInfo = null;

            var newStep = new MockPropertyStep<int>();
            newStep.Get.Func(p =>
            {
                sentMockInfo = p;
                return 5;
            });
            ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep(newStep);

            int value = _propertyMock.Value;
            Assert.Equal(5, value);
            Assert.Same(_propertyMock, sentMockInfo);
        }

        [Fact]
        public void send_mock_information_and_value_to_step_on_setting()
        {
            IMockInfo sentMockInfo = null;
            int sentValue = 0;

            var newStep = new MockPropertyStep<int>();
            newStep.Set.Action(p =>
            {
                sentMockInfo = p.mockInfo;
                sentValue = p.value;
            });
            ((ICanHaveNextPropertyStep<int>)_propertyMock).SetNextStep(newStep);

            _propertyMock.Value = 5;

            Assert.Same(_propertyMock, sentMockInfo);
            Assert.Equal(5, sentValue);
        }
    }
}
