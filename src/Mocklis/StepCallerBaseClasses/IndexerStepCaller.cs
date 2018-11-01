// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.StepCallerBaseClasses
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public abstract class IndexerStepCaller<TKey, TValue> : IIndexerStepCaller<TKey, TValue>
    {
        public IIndexerStep<TKey, TValue> NextStep { get; private set; } = MissingIndexerStep<TKey, TValue>.Instance;

        public TImplementation SetNextStep<TImplementation>(TImplementation step) where TImplementation : IIndexerStep<TKey, TValue>
        {
            NextStep = step;
            return step;
        }
    }
}
