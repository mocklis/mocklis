// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock_this_should.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core.Tests.Core
{
    #region Using Directives

    using Mocklis.Core.Tests.Mocks;
    using Xunit;

    #endregion

    public class IndexerMock_this_should
    {
        private readonly IndexerMock<int, string> _indexerMock;

        public IndexerMock_this_should()
        {
            _indexerMock = new IndexerMock<int, string>(new object(), "ClassName", "InterfaceName", "MemberName", "MockName");
        }

        [Fact]
        public void send_mock_information_and_key_to_step_and_get_value_on_getting()
        {
            IMockInfo sentMockInfo = null;
            int sentKey = 0;
            var newStep = new MockIndexerStep<int, string>();
            newStep.Get.Func(p =>
            {
                sentMockInfo = p.mockInfo;
                sentKey = p.key;
                return "5";
            });
            ((IIndexerStepCaller<int, string>)_indexerMock).SetNextStep(newStep);

            string value = _indexerMock[5];

            Assert.Same(_indexerMock, sentMockInfo);
            Assert.Equal(5, sentKey);
            Assert.Equal("5", value);
        }

        [Fact]
        public void send_mock_information_key_and_value_to_step_on_setting()
        {
            IMockInfo sentMockInfo = null;
            int sentKey = 0;
            string sentValue = null;

            var newStep = new MockIndexerStep<int, string>();
            newStep.Set.Action(p =>
            {
                sentMockInfo = p.mockInfo;
                sentKey = p.key;
                sentValue = p.value;
            });
            ((IIndexerStepCaller<int, string>)_indexerMock).SetNextStep(newStep);

            _indexerMock[5] = "5";

            Assert.Same(_indexerMock, sentMockInfo);
            Assert.Equal(5, sentKey);
            Assert.Equal("5", sentValue);
        }
    }
}
