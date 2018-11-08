// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexerStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    public interface IIndexerStepCaller<out TKey, TValue>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TStep SetNextStep<TStep>(TStep step) where TStep : IIndexerStep<TKey, TValue>;
    }
}
