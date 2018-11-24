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
        public void send_mock_instance_to_step()
        {
            MemberMock sentInstance = null;

            var newStep = new MockMethodStep<ValueTuple, ValueTuple>();
            newStep.Call.Action(p => { sentInstance = p.memberMock; });
            _parameterLessActionMock.SetNextStep(newStep);

            _parameterLessActionMock.Call();

            Assert.Same(_parameterLessActionMock, sentInstance);
        }

        [Fact]
        public void send_mock_instance_and_parameters_to_step()
        {
            MemberMock sentInstance = null;
            int sentParam = 0;

            var newStep = new MockMethodStep<int, ValueTuple>();
            newStep.Call.Action(p =>
            {
                sentInstance = p.memberMock;
                sentParam = p.param;
            });
            _actionMock.SetNextStep(newStep);

            _actionMock.Call(5);

            Assert.Same(_actionMock, sentInstance);
            Assert.Equal(5, sentParam);
        }
    }
}
