// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockStoredProperty.cs">
//   SPDX-License-Identifier: MIT
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
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockStoredProperty()
        {
            Value = new PropertyMock<TValue>(this, "MockStoredProperty", "IStoredProperty", "Value", "Value", Strictness.Lenient);
        }

        public PropertyMock<TValue> Value { get; }

        TValue IStoredProperty<TValue>.Value
        {
            get => Value.Value;
            set => Value.Value = value;
        }
    }
}
