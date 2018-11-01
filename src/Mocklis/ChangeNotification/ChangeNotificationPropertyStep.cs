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

        public TValue Get(MemberMock memberMock)
        {
            return NextStep.Get(memberMock);
        }

        public void Set(MemberMock memberMock, TValue value)
        {
            if (!Comparer.Equals(NextStep.Get(memberMock), value))
            {
                NextStep.Set(memberMock, value);
                _propertyChangedEvent.Raise(null, memberMock.MemberName);
            }
        }
    }
}
