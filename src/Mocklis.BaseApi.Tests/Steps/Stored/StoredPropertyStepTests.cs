// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class StoredPropertyStepTests
    {
        public MockMembers MockMembers { get; } = new MockMembers();
        public IProperties Sut => MockMembers;

        [Fact]
        public void BeBothResultAndOutParameter()
        {
            var step = MockMembers.StringProperty.Stored(out var store, "");
            Assert.Same(step, store);
        }

        [Fact]
        public void ReturnStoredValues()
        {
            MockMembers.StringProperty.Stored("");
            Sut.StringProperty = "Hello";

            Assert.Equal("Hello", Sut.StringProperty);
        }

        [Fact]
        public void ReturnDefaultForMissingValues()
        {
            var step = MockMembers.IntProperty.Stored();
            Assert.Null(Sut.StringProperty);
            Assert.Equal(0, step.Value);
        }

        [Fact]
        public void AllowExternalModification()
        {
            MockMembers.StringProperty.Stored(out var store, "");

            Sut.StringProperty = "Hello";
            var value1 = store.Value;
            store.Value = "World";
            var value2 = Sut.StringProperty;

            Assert.Equal("Hello", value1);
            Assert.Equal("World", value2);
        }

        [Fact]
        public void AllowSettingAnInitialValue()
        {
            MockMembers.StringProperty.Stored("Test");

            Assert.Equal("Test", Sut.StringProperty);
        }

        [Fact]
        public void HandleChangeNotificationWithoutComparer()
        {
            var mock = new MockPropertiesWithChangeNotification();
            mock.PropertyChanged.Stored(out var propertyChangedEvent);
            mock.StringProperty.StoredWithChangeNotification(propertyChangedEvent);
            IProperties sut = mock;

            var updatedProperties = new List<string>();

            mock.PropertyChanged.Add((s, e) =>
            {
                updatedProperties.Add(e.PropertyName);
            });

            sut.StringProperty = "Hello";

            Assert.Equal(new[] { "StringProperty" }, updatedProperties);
        }

        [Fact]
        public void HandleChangeNotificationWithComparer()
        {
            var mock = new MockPropertiesWithChangeNotification();
            mock.PropertyChanged.Stored(out var propertyChangedEvent);
            mock.StringProperty.StoredWithChangeNotification(out var prop, propertyChangedEvent, "Test", new StringLengthComparer());
            IProperties sut = mock;

            var updatedProperties = new List<string>();

            mock.PropertyChanged.Add((s, e) =>
            {
                updatedProperties.Add(e.PropertyName);
            });

            // This causes an update as 'Hello' doesn't have the same length as the current value 'Test'
            sut.StringProperty = "Hello";

            // This doesn't cause an update as 'World' has the same length as current value 'Hello'.
            sut.StringProperty = "World";

            // So we'll only see one update
            Assert.Equal(new[] { "StringProperty" }, updatedProperties);

            // And the last written value was 'Hello'
            Assert.Equal("Hello", prop.Value);
        }
    }
}
