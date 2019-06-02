// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesEventStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Times
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Xunit;

    #endregion

    public class TimesEventStep_should
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IEvents Sut => MockMembers;
        private readonly EventHandler _handler = (sender, args) => { };

        [Fact]
        public void UseSameCounterForAddsAndRemoves()
        {
            IReadOnlyList<EventHandler> collectedAdds = null;
            IReadOnlyList<EventHandler> collectedRemoves = null;
            MockMembers.MyEvent
                .Times(4, step => step
                    .RecordBeforeAdd(out collectedAdds)
                    .RecordBeforeRemove(out collectedRemoves));

            Sut.MyEvent += _handler;
            Sut.MyEvent -= _handler;
            Sut.MyEvent += _handler;
            Sut.MyEvent -= _handler;
            Sut.MyEvent += _handler;
            Sut.MyEvent -= _handler;


            Assert.Equal(new[] { _handler, _handler }, collectedAdds);
            Assert.Equal(new[] { _handler, _handler }, collectedAdds.ToArray());
            Assert.Equal(new[] { _handler, _handler }, collectedRemoves);
            Assert.Equal(new[] { _handler, _handler }, collectedRemoves.ToArray());
        }
    }
}
