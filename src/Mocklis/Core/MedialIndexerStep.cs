// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MedialIndexerStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

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

        public virtual TValue Get(object instance, MemberMock memberMock, TKey key)
        {
            return NextStep.Get(instance, memberMock, key);
        }

        public virtual void Set(object instance, MemberMock memberMock, TKey key, TValue value)
        {
            NextStep.Set(instance, memberMock, key, value);
        }
    }
}
