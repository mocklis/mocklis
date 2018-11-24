// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MedialIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using Mocklis.Steps.Missing;

    #endregion

    public class MedialIndexerStep<TKey, TValue> : IIndexerStep<TKey, TValue>, IIndexerStepCaller<TKey, TValue>
    {
        protected IIndexerStep<TKey, TValue> NextStep { get; private set; } = MissingIndexerStep<TKey, TValue>.Instance;

        public TStep SetNextStep<TStep>(TStep step) where TStep : IIndexerStep<TKey, TValue>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public virtual TValue Get(MemberMock memberMock, TKey key)
        {
            return NextStep.Get(memberMock, key);
        }

        public virtual void Set(MemberMock memberMock, TKey key, TValue value)
        {
            NextStep.Set(memberMock, key, value);
        }
    }
}
