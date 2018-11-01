// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeNotificationPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.ChangeNotification
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.StepCallerBaseClasses;

    #endregion

    public class ChangeNotificationPropertyStep<TValue> : PropertyStepCaller<TValue>, IPropertyStep<TValue>
    {
        private readonly PropertyChangedEventStep _propertyChangedEvent;
        private IEqualityComparer<TValue> Comparer { get; }

        public ChangeNotificationPropertyStep(PropertyChangedEventStep propertyChangedEvent)
        {
            _propertyChangedEvent = propertyChangedEvent;
            Comparer = EqualityComparer<TValue>.Default;
        }

        public TValue Get(object instance, MemberMock memberMock)
        {
            return NextStep.Get(instance, memberMock);
        }

        public void Set(object instance, MemberMock memberMock, TValue value)
        {
            if (!Comparer.Equals(NextStep.Get(instance, memberMock), value))
            {
                NextStep.Set(instance, memberMock, value);
                _propertyChangedEvent.Raise(instance, memberMock.MemberName);
            }
        }
    }
}
