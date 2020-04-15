// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaisePropertyChangedEventPropertyStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Miscellaneous
{
    #region Using Directives

    using System;
    using System.ComponentModel;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     This class represents a step that raises a <see cref="PropertyChangedEventHandler" /> event on every write.
    ///     Inherits from the <see cref="PropertyStepWithNext{TValue}" /> class.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="PropertyStepWithNext{TValue}" />
    public class RaisePropertyChangedEventPropertyStep<TValue> : PropertyStepWithNext<TValue>
    {
        private readonly IStoredEvent<PropertyChangedEventHandler> _propertyChangedEvent;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RaisePropertyChangedEventPropertyStep{TValue}" /> class.
        /// </summary>
        /// <param name="propertyChangedEvent">The event store that is used to raise the event.</param>
        public RaisePropertyChangedEventPropertyStep(IStoredEvent<PropertyChangedEventHandler> propertyChangedEvent)
        {
            _propertyChangedEvent = propertyChangedEvent ?? throw new ArgumentNullException(nameof(propertyChangedEvent));
        }

        /// <summary>
        ///     Called when a value is written to the property.
        ///     This implementation raises an event when the value has been written.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the value is written.</param>
        /// <param name="value">The value being written.</param>
        public override void Set(IMockInfo mockInfo, TValue value)
        {
            base.Set(mockInfo, value);
            _propertyChangedEvent.Raise(mockInfo.MockInstance, new PropertyChangedEventArgs(mockInfo.MemberName));
        }
    }
}
