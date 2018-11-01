// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.ChangeNotification
{
    #region Using Directives

    using System.ComponentModel;
    using Mocklis.Core;

    #endregion

    public static class MockExtensions
    {
        public static PropertyChangedEventStep PropertyChangedEvent(
            this IEventStepCaller<PropertyChangedEventHandler> caller,
            out PropertyChangedEventStep step)
        {
            step = new PropertyChangedEventStep();
            return caller.SetNextStep(step);
        }

        public static ChangeNotificationPropertyStep<TValue> WithChangeNotification<TValue>(
            this IPropertyStepCaller<TValue> caller,
            PropertyChangedEventStep propertyChangedEvent)
        {
            return caller.SetNextStep(new ChangeNotificationPropertyStep<TValue>(propertyChangedEvent));
        }
    }
}
