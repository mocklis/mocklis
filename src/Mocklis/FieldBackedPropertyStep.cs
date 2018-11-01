// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FieldBackedPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class FieldBackedPropertyStep<TValue> : IPropertyStep<TValue>
    {
        public TValue Value { get; set; }

        public FieldBackedPropertyStep(TValue initialValue = default)
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
