// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that represents a mock of an event of a give type. This class cannot be inherited.
    ///     Inherits from the <see cref="MemberMock" /> class.
    ///     Implements the <see cref="ICanHaveNextEventStep{THandler}" /> interface.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    /// <seealso cref="MemberMock" />
    /// <seealso cref="ICanHaveNextEventStep{THandler}" />
    public sealed class EventMock<THandler> : MemberMock, ICanHaveNextEventStep<THandler> where THandler : Delegate
    {
        private IEventStep<THandler>? _nextStep;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EventMock{THandler}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock with behaviour.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        public EventMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
            string memberMockName, Strictness strictness)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName, strictness)
        {
        }

        /// <summary>
        ///     Replaces the current 'next' step with a new step.
        /// </summary>
        /// <typeparam name="TStep">The actual type of the new step.</typeparam>
        /// <param name="step">The new step.</param>
        /// <returns>The new step, so that we can add further steps in a fluent fashion.</returns>
        TStep ICanHaveNextEventStep<THandler>.SetNextStep<TStep>(TStep step)
        {
            if (step == null)
            {
                throw new ArgumentNullException(nameof(step));
            }

            _nextStep = step;
            return step;
        }

        /// <summary>
        ///     Restores the Mock to an unconfigured state.
        /// </summary>
        public override void Clear()
        {
            _nextStep = null;
        }

        /// <summary>
        ///     Adds an event handler to the mocked event.
        /// </summary>
        /// <remarks>
        ///     This method is called when the event is called through a mocked interface, but can also be used to interact with
        ///     the mock directly.
        /// </remarks>
        /// <param name="value">The event handler.</param>
        public void Add(THandler? value)
        {
            if (_nextStep == null)
            {
                IMockInfo mockInfo = this;

                if (mockInfo.Strictness == Strictness.Lenient)
                {
                    return;
                }

                throw new MockMissingException(MockType.EventAdd, mockInfo);
            }

            _nextStep.Add(this, value);
        }

        /// <summary>
        ///     Removes an event handler from the mocked event.
        /// </summary>
        /// <remarks>
        ///     This method is called when the event is called through a mocked interface, but can also be used to interact with
        ///     the mock directly.
        /// </remarks>
        /// <param name="value">The event handler.</param>
        public void Remove(THandler? value)
        {
            if (_nextStep == null)
            {
                IMockInfo mockInfo = this;

                if (mockInfo.Strictness == Strictness.Lenient)
                {
                    return;
                }

                throw new MockMissingException(MockType.EventRemove, mockInfo);
            }

            _nextStep.Remove(this, value);
        }
    }
}
