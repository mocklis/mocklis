// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockPropertiesWithChangeNotification.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockPropertiesWithChangeNotification()
        {
            StringProperty = new PropertyMock<string>(this, "MockPropertiesWithChangeNotification", "IProperties", "StringProperty", "StringProperty", Strictness.Lenient);
            IntProperty = new PropertyMock<int>(this, "MockPropertiesWithChangeNotification", "IProperties", "IntProperty", "IntProperty", Strictness.Lenient);
            BoolProperty = new PropertyMock<bool>(this, "MockPropertiesWithChangeNotification", "IProperties", "BoolProperty", "BoolProperty", Strictness.Lenient);
            DateTimeProperty = new PropertyMock<System.DateTime>(this, "MockPropertiesWithChangeNotification", "IProperties", "DateTimeProperty", "DateTimeProperty", Strictness.Lenient);
            PropertyChanged = new EventMock<PropertyChangedEventHandler>(this, "MockPropertiesWithChangeNotification", "INotifyPropertyChanged", "PropertyChanged", "PropertyChanged", Strictness.Lenient);
        }

        public PropertyMock<string> StringProperty { get; }
        string IProperties.StringProperty { get => StringProperty.Value; set => StringProperty.Value = value; }
        public PropertyMock<int> IntProperty { get; }
        int IProperties.IntProperty { get => IntProperty.Value; set => IntProperty.Value = value; }
        public PropertyMock<bool> BoolProperty { get; }
        bool IProperties.BoolProperty { get => BoolProperty.Value; set => BoolProperty.Value = value; }
        public PropertyMock<System.DateTime> DateTimeProperty { get; }
        System.DateTime IProperties.DateTimeProperty { get => DateTimeProperty.Value; set => DateTimeProperty.Value = value; }
        public EventMock<PropertyChangedEventHandler> PropertyChanged { get; }

        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged { add => PropertyChanged.Add(value); remove => PropertyChanged.Remove(value); }
    }
}
