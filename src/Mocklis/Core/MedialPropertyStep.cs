// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MedialPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public class MedialPropertyStep<TValue> : IPropertyStep<TValue>, IPropertyStepCaller<TValue>
    {
        protected IPropertyStep<TValue> NextStep { get; private set; } = MissingPropertyStep<TValue>.Instance;

        public TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>
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
