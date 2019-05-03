// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockPropertiesWithChangeNotification.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Tests.Interfaces;

    #endregion

    [MocklisClass]
    public class MockPropertiesWithChangeNotification : IProperties, INotifyPropertyChanged
    {
        public MockPropertiesWithChangeNotification()
        {
            StringProperty =
                new PropertyMock<string>(this, "MockPropertiesWithChangeNotification", "IProperties", "StringProperty", "StringProperty");
            IntProperty = new PropertyMock<int>(this, "MockPropertiesWithChangeNotification", "IProperties", "IntProperty", "IntProperty");
            BoolProperty = new PropertyMock<bool>(this, "MockPropertiesWithChangeNotification", "IProperties", "BoolProperty", "BoolProperty");
            PropertyChanged = new EventMock<PropertyChangedEventHandler>(this, "MockPropertiesWithChangeNotification", "INotifyPropertyChanged",
                "PropertyChanged", "PropertyChanged");
        }

        public PropertyMock<string> StringProperty { get; }
        string IProperties.StringProperty { get => StringProperty.Value; set => StringProperty.Value = value; }
        public PropertyMock<int> IntProperty { get; }
        int IProperties.IntProperty { get => IntProperty.Value; set => IntProperty.Value = value; }
        public PropertyMock<bool> BoolProperty { get; }
        bool IProperties.BoolProperty { get => BoolProperty.Value; set => BoolProperty.Value = value; }
        public EventMock<PropertyChangedEventHandler> PropertyChanged { get; }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChanged.Add(value);
            remove => PropertyChanged.Remove(value);
        }
    }
}
