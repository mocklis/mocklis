// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMock_Call_should.cs">
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

    public class ActionMethodMock_Call_should
    {
        private readonly ActionMethodMock _parameterLessActionMock;
        private readonly ActionMethodMock<int> _actionMock;

        public ActionMethodMock_Call_should()
        {
            _parameterLessActionMock = new ActionMethodMock(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
            _actionMock = new ActionMethodMock<int>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_information_to_step()
        {
            IMockInfo sentMockInfo = null;

            var newStep = new MockMethodStep<ValueTuple, ValueTuple>();
            newStep.Call.Action(p => { sentMockInfo = p.mockInfo; });
            _parameterLessActionMock.SetNextStep(newStep);

            _parameterLessActionMock.Call();

            Assert.Same(_parameterLessActionMock, sentMockInfo);
        }

        [Fact]
        public void send_mock_information_and_parameters_to_step()
        {
            IMockInfo sentMockInfo = null;
            int sentParam = 0;

            var newStep = new MockMethodStep<int, ValueTuple>();
            newStep.Call.Action(p =>
            {
                sentMockInfo = p.mockInfo;
                sentParam = p.param;
            });
            _actionMock.SetNextStep(newStep);

            _actionMock.Call(5);

            Assert.Same(_actionMock, sentMockInfo);
            Assert.Equal(5, sentParam);
        }
    }
}
