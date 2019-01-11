// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanHaveNextMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    public interface ICanHaveNextMethodStep<out TParam, in TResult>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>;
    }
}
