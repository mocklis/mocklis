// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockStoredProperty.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Mocks
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    [MocklisClass]
    public class MockStoredProperty<TValue> : IStoredProperty<TValue>
    {
        public MockStoredProperty()
        {
            Value = new PropertyMock<TValue>(this, "MockStoredProperty", "IStoredProperty", "Value", "Value");
        }

        public PropertyMock<TValue> Value { get; }

        TValue IStoredProperty<TValue>.Value
        {
            get => Value.Value;
            set => Value.Value = value;
        }
    }
}
