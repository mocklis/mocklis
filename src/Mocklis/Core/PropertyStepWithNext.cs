// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyStepWithNext.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public class PropertyStepWithNext<TValue> : IPropertyStep<TValue>, ICanHaveNextPropertyStep<TValue>
    {
        protected IPropertyStep<TValue> NextStep { get; private set; } = MissingPropertyStep<TValue>.Instance;

        TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public virtual TValue Get(IMockInfo mockInfo)
        {
            return NextStep.Get(mockInfo);
        }

        public virtual void Set(IMockInfo mockInfo, TValue value)
        {
            NextStep.Set(mockInfo, value);
        }
    }
}
