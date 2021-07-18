// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimesEventStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Times
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class TimesEventStepTests
    {
        private MockMembers MockMembers { get; } = new MockMembers();
        private IEvents Sut => MockMembers;
        private readonly EventHandler _handler = (sender, args) => { };

        [Fact]
        public void UseSameCounterForAddsAndRemoves()
        {
            IReadOnlyList<EventHandler?>? collectedAdds = null;
            IReadOnlyList<EventHandler?>? collectedRemoves = null;
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
            Assert.Equal(new[] { _handler, _handler }, collectedAdds?.ToArray());
            Assert.Equal(new[] { _handler, _handler }, collectedRemoves);
            Assert.Equal(new[] { _handler, _handler }, collectedRemoves?.ToArray());
        }
    }
}
