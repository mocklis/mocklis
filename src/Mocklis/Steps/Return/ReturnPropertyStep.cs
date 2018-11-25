// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnPropertyStep<TValue> : IPropertyStep<TValue>
    {
        private readonly TValue _value;

        public ReturnPropertyStep(TValue value)
        {
            _value = value;
        }

        public TValue Get(IMockInfo mockInfo)
        {
            return _value;
        }

        public void Set(IMockInfo mockInfo, TValue value)
        {
        }
    }
}
