// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaisePropertyChangedEventPropertyStep_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Steps.Miscellaneous
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using Mocklis.Steps.Miscellaneous;
    using Mocklis.Tests.Interfaces;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Xunit;

    #endregion

    public class RaisePropertyChangedEventPropertyStep_should
    {
        private readonly MockPropertiesWithChangeNotification _mockProperties;
        private readonly IProperties _properties;
        private readonly IStoredEvent<PropertyChangedEventHandler> _npc;
        private readonly List<string> _changedPropertyNames = new List<string>();

        public RaisePropertyChangedEventPropertyStep_should()
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
                var _ = new RaisePropertyChangedEventPropertyStep<EventHandler>(null);
            });

            Assert.Equal("propertyChangedEvent", exception.ParamName);
        }

        [Fact]
        private void RaisePropertyChangedEventCorrectly()
        {
            // Arrange
            _mockProperties.StringProperty.RaisePropertyChangedEvent(_npc).Dummy();

            // Act
            _properties.StringProperty = "Hello";

            // Assert
            Assert.Equal(new[] { "StringProperty" }, _changedPropertyNames);
        }
    }
}
