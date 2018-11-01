// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public sealed class IndexerMock<TKey, TValue> : MemberMock, IIndexerStepCaller<TKey, TValue>
    {
        public IIndexerStep<TKey, TValue> NextStep { get; private set; } = MissingIndexerStep<TKey, TValue>.Instance;

        public IndexerMock(string interfaceName, string memberName, string memberMockName) : base(interfaceName, memberName, memberMockName)
        {
        }

        public TStep SetNextStep<TStep>(TStep step) where TStep : IIndexerStep<TKey, TValue>
        {
            NextStep = step;
            return step;
        }

        public TValue Get(TKey key)
        {
            return NextStep.Get(this, key);
        }

        public void Set(TKey key, TValue value)
        {
            NextStep.Set(this, key, value);
        }
    }
}
