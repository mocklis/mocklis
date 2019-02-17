// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DummyEventStep_Remove_should.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Dummy
{
    #region Using Directives

    using System;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class DummyEventStep_Remove_should
    {
        private void MyEventHandler(object sender, EventArgs e)
        {
        }

        private readonly MockMembers _mockMembers = new MockMembers();

        [Fact]
        public void not_do_anything()
        {
            _mockMembers.MyEvent.Dummy();
            ((IEvents)_mockMembers).MyEvent -= MyEventHandler;
        }
    }
}
