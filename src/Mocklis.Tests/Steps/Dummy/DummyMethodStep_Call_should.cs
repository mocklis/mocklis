// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyMethodStep_Call_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Dummy
{
    #region Using Directives

    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class DummyMethodStep_Call_should
    {
        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void return_default_value()
        {
            _mockMembers.DoStuff.Dummy();
            var result = ((IMethods)_mockMembers).DoStuff(150);
            Assert.Equal(default, result);
        }
    }
}