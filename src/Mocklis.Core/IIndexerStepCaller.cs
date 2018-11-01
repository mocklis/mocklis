// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IIndexerStepCaller<TKey, TValue>
    {
        IIndexerStep<TKey, TValue> NextStep { get; }
        TStep SetNextStep<TStep>(TStep step) where TStep : IIndexerStep<TKey, TValue>;
    }
}
