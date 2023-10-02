// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that represents a mock of a property of a given type. This class cannot be inherited.
    ///     Inherits from the <see cref="MemberMock" /> class.
    ///     Implements the <see cref="ICanHaveNextPropertyStep{TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TValue">The type of the property.</typeparam>
    /// <seealso cref="MemberMock" />
    /// <seealso cref="ICanHaveNextPropertyStep{TValue}" />
    public sealed class PropertyMock<TValue> : MemberMock, ICanHaveNextPropertyStep<TValue>
    {
        private IPropertyStep<TValue>? _nextStep;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PropertyMock{TValue}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        public PropertyMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
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
        /// <exception cref="ArgumentNullException">step</exception>
        TStep ICanHaveNextPropertyStep<TValue>.SetNextStep<TStep>(TStep step)
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
        ///     Gets or sets the <typeparamref name="TValue" /> on the mocked property.
        /// </summary>
        /// <value>The value being read or written.</value>
        public TValue Value
        {
            get
            {
                if (_nextStep == null)
                {
                    IMockInfo mockInfo = this;

                    if (mockInfo.Strictness == Strictness.Lenient)
                    {
                        // Well yes - in lenient mode even non-nullable reference types will get a null value.
                        return default!;
                    }

                    throw new MockMissingException(MockType.PropertyGet, mockInfo);
                }

                return _nextStep.Get(this);
            }
            set
            {
                if (_nextStep == null)
                {
                    IMockInfo mockInfo = this;

                    if (mockInfo.Strictness == Strictness.Lenient)
                    {
                        return;
                    }

                    throw new MockMissingException(MockType.PropertySet, mockInfo);
                }

                _nextStep.Set(this, value);
            }
        }
    }
}
