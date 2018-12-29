// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MiscellaneousStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Steps.Miscellaneous;
    using Mocklis.Verification;

    #endregion

    public static class MiscellaneousStepExtensions
    {
        public static ICanHaveNextPropertyStep<TValue> RaisePropertyChangedEvent<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent)
        {
            return caller.SetNextStep(new RaisePropertyChangedEventPropertyStep<TValue>(propertyChangedEvent));
        }
    }
}
