// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaisePropertyChangedEventPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Miscellaneous
{
    #region Using Directives

    using System.ComponentModel;
    using Mocklis.Core;

    #endregion

    public class RaisePropertyChangedEventPropertyStep<TValue> : MedialPropertyStep<TValue>
    {
        private readonly IStoredEvent<PropertyChangedEventHandler> _propertyChangedEvent;

        public RaisePropertyChangedEventPropertyStep(IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent)
        {
            _propertyChangedEvent = propertyChangedEvent;
        }

        public override void Set(object instance, MemberMock memberMock, TValue value)
        {
            base.Set(instance, memberMock, value);
            _propertyChangedEvent.EventHandler?.Invoke(instance, new PropertyChangedEventArgs(memberMock.MemberName));
        }
    }
}
