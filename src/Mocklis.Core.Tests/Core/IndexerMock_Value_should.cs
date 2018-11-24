// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock_Value_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class IndexerMock_Value_should
    {
        private readonly IndexerMock<int, string> _indexerMock;

        public IndexerMock_Value_should()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_instance_and_key_to_step_and_get_value_on_getting()
        {
            MemberMock sentInstance = null;
            int sentKey = 0;
            var newStep = new MockIndexerStep<int, string>();
            newStep.Get.Func(p =>
            {
                sentInstance = p.memberMock;
                sentKey = p.key;
                return "5";
            });
            _indexerMock.SetNextStep(newStep);

            string value = _indexerMock[5];

            Assert.Same(_indexerMock, sentInstance);
            Assert.Equal(5, sentKey);
            Assert.Equal("5", value);
        }

        [Fact]
        public void send_mock_instance_key_and_value_to_step_on_setting()
        {
            MemberMock sentInstance = null;
            int sentKey = 0;
            string sentValue = null;

            var newStep = new MockIndexerStep<int, string>();
            newStep.Set.Action(p =>
            {
                sentInstance = p.memberMock;
                sentKey = p.key;
                sentValue = p.value;
            });
            _indexerMock.SetNextStep(newStep);

            _indexerMock[5] = "5";

            Assert.Same(_indexerMock, sentInstance);
            Assert.Equal(5, sentKey);
            Assert.Equal("5", sentValue);
        }
    }
}
