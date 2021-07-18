// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JoinStepExtensionsTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class JoinStepExtensionsTests
    {
        private MockMembers Targets { get; } = new MockMembers();
        private MockMembers Sources { get; } = new MockMembers();
        private IEvents Events { get; }
        private IIndexers Indexers { get; }
        private IMethods Methods { get; }
        private IProperties Properties { get; }

        private readonly EventHandler _handler = (sender, args) => { };

        public JoinStepExtensionsTests()
        {
            Events = Sources;
            Indexers = Sources;
            Methods = Sources;
            Properties = Sources;
        }

        [Fact]
        public void EventAddFollowsJoin()
        {
            Targets.MyEvent.JoinPoint(out var joinPoint).InstanceRecordBeforeAdd(out var ledger, (i, h) => (i, h));
            Sources.MyEvent.Join(joinPoint);

            Events.MyEvent += _handler;

            var (instance, handler) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Same(_handler, handler);
        }

        [Fact]
        public void EventRemoveFollowsJoin()
        {
            Targets.MyEvent.JoinPoint(out var joinPoint).InstanceRecordBeforeRemove(out var ledger, (i, h) => (i, h));
            Sources.MyEvent.Join(joinPoint);

            Events.MyEvent -= _handler;

            var (instance, handler) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Same(_handler, handler);
        }

        [Fact]
        public void IndexerGetFollowsJoin()
        {
            Targets.Item
                .JoinPoint(out var joinPoint)
                .InstanceRecordAfterGet(out var ledger, (i, k, v) => (i, k, v))
                .Return("Twentyfive");
            Sources.Item.Join(joinPoint);

            var result = Indexers[25];

            var (instance, key, value) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Equal(25, key);
            Assert.Equal("Twentyfive", value);
            Assert.Equal("Twentyfive", result);
        }

        [Fact]
        public void IndexerSetFollowsJoin()
        {
            Targets.Item
                .JoinPoint(out var joinPoint)
                .InstanceRecordBeforeSet(out var ledger, (i, k, v) => (i, k, v));
            Sources.Item.Join(joinPoint);

            Indexers[25] = "Twentyfive";

            var (instance, key, value) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Equal(25, key);
            Assert.Equal("Twentyfive", value);
        }

        [Fact]
        public void MethodCallFollowsJoin()
        {
            Targets.FuncWithParameter
                .JoinPoint(out var joinPoint)
                .InstanceRecordAfterCall(out var ledger, (i, p, r) => (i, p, r))
                .Func(a => a * a);
            Sources.FuncWithParameter.Join(joinPoint);

            Methods.FuncWithParameter(5);

            var (instance, parameters, result) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Equal(5, parameters);
            Assert.Equal(25, result);
        }

        [Fact]
        public void PropertyGetFollowsJoin()
        {
            Targets.StringProperty
                .JoinPoint(out var joinPoint)
                .InstanceRecordAfterGet(out var ledger, (i, v) => (i, v))
                .Return("Twentyfive");
            Sources.StringProperty.Join(joinPoint);

            var result = Properties.StringProperty;

            var (instance, value) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Equal("Twentyfive", value);
            Assert.Equal("Twentyfive", result);
        }

        [Fact]
        public void PropertySetFollowsJoin()
        {
            Targets.StringProperty
                .JoinPoint(out var joinPoint)
                .InstanceRecordBeforeSet(out var ledger, (i, v) => (i, v));
            Sources.StringProperty.Join(joinPoint);

            Properties.StringProperty = "Twentyfive";

            var (instance, value) = Assert.Single(ledger);
            Assert.Same(Sources, instance);
            Assert.Equal("Twentyfive", value);
        }
    }
}
