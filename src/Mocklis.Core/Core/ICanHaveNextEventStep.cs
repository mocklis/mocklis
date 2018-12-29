// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICanHaveNextEventStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;
    using System.ComponentModel;

    #endregion

    public interface ICanHaveNextEventStep<out THandler> where THandler : Delegate
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        TStep SetNextStep<TStep>(TStep step) where TStep : IEventStep<THandler>;
    }
}
