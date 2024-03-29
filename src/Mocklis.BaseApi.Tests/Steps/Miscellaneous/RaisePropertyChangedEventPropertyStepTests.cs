// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaisePropertyChangedEventPropertyStepTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Miscellaneous
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class RaisePropertyChangedEventPropertyStepTests
    {
        private readonly MockPropertiesWithChangeNotification _mockProperties;
        private readonly IProperties _properties;
        private readonly IStoredEvent<PropertyChangedEventHandler> _npc;
        private readonly List<string?> _changedPropertyNames = new List<string?>();

        public RaisePropertyChangedEventPropertyStepTests()
        {
            _mockProperties = new MockPropertiesWithChangeNotification();
            _properties = _mockProperties;

            _npc = _mockProperties.PropertyChanged.Stored();

            ((INotifyPropertyChanged)_mockProperties).PropertyChanged += (sender, args) =>
            {
                _changedPropertyNames.Add(args.PropertyName);
            };
        }

        [Fact]
        public void RequirePropertyChangedEvent()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var _ = new RaisePropertyChangedEventPropertyStep<EventHandler>(null!);
            });

            Assert.Equal("propertyChangedEvent", exception.ParamName);
        }

        [Fact]
        public void RaisePropertyChangedEventCorrectly()
        {
            // Arrange
            _mockProperties.StringProperty.RaisePropertyChangedEvent(_npc);

            // Act
            _properties.StringProperty = "Hello";

            // Assert
            Assert.Equal(new[] { "StringProperty" }, _changedPropertyNames);
        }
    }
}
