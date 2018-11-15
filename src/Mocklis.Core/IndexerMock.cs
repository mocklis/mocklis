// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public sealed class IndexerMock<TKey, TValue> : MemberMock, IIndexerStepCaller<TKey, TValue>
    {
        public IIndexerStep<TKey, TValue> NextStep { get; private set; } = MissingIndexerStep<TKey, TValue>.Instance;

        public IndexerMock(object mockInstance, string interfaceName, string memberName, string memberMockName) : base(mockInstance, interfaceName,
            memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IIndexerStep<TKey, TValue>
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            NextStep = step;
            return step;
        }

        public TValue this[TKey key]
        {
            get => NextStep.Get(MockInstance, this, key);
            set => NextStep.Set(MockInstance, this, key, value);
        }
    }
}
