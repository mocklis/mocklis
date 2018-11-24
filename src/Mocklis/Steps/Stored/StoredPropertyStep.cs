// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Stored
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    public class StoredPropertyStep<TValue> : IPropertyStep<TValue>, IStoredProperty<TValue>
    {
        public TValue Value { get; set; }

        public StoredPropertyStep(TValue initialValue = default)
        {
            Value = initialValue;
        }

        public TValue Get(MemberMock memberMock)
        {
            return Value;
        }

        public void Set(MemberMock memberMock, TValue value)
        {
            Value = value;
        }
    }
}
