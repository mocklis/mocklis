// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Lambda
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    public class FuncPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly Func<TValue> _func;

        public FuncPropertyStep(Func<TValue> func)
        {
            _func = func;
        }

        public override TValue Get(IMockInfo mockInfo)
        {
            return _func();
        }
    }
}
