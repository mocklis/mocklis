// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanHaveNextPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System.ComponentModel;

    #endregion

    public interface ICanHaveNextPropertyStep<TValue>
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>;
    }
}
