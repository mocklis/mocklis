// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class ReturnPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly TValue _value;

        public ReturnPropertyStep(TValue value)
        {
            _value = value;
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            return _value;
        }
    }
}
