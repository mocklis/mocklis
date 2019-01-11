// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaisePropertyChangedEventPropertyStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Miscellaneous
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using Mocklis.Core;
    using Mocklis.Verification;

    #endregion

    public class RaisePropertyChangedEventPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly IStoredEvent<PropertyChangedEventHandler> _propertyChangedEvent;

        public RaisePropertyChangedEventPropertyStep(IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent)
        {
            _propertyChangedEvent = propertyChangedEvent ?? throw new ArgumentNullException(nameof(propertyChangedEvent));
        }

        public override void Set(IMockInfo mockInfo, TValue value)
        {
            base.Set(mockInfo, value);
            _propertyChangedEvent.Raise(mockInfo.MockInstance, new PropertyChangedEventArgs(mockInfo.MemberName));
        }
    }
}
