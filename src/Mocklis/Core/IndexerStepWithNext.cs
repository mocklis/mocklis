// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepWithNext.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public class IndexerStepWithNext<TKey, TValue> : IIndexerStep<TKey, TValue>, ICanHaveNextIndexerStep<TKey, TValue>
    {
        protected IIndexerStep<TKey, TValue> NextStep { get; private set; } = MissingIndexerStep<TKey, TValue>.Instance;

        TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public virtual TValue Get(IMockInfo mockInfo, TKey key)
        {
            return NextStep.Get(mockInfo, key);
        }

        public virtual void Set(IMockInfo mockInfo, TKey key, TValue value)
        {
            NextStep.Set(mockInfo, key, value);
        }
    }
}
