// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StoredPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Stored
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class StoredPropertyStep<TValue> : IPropertyStep<TValue>, IStoredProperty<TValue>
    {
        public TValue Value { get; set; }

        public StoredPropertyStep(TValue initialValue = default)
        {
            Value = initialValue;
        }

        public TValue Get(object instance, MemberMock memberMock)
        {
            return Value;
        }

        public void Set(object instance, MemberMock memberMock, TValue value)
        {
            Value = value;
        }
    }
}
