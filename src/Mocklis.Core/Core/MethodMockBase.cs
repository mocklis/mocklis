// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MethodMockBase.cs">
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
    ///     Abstract class that represents a mock of a method of a given type.
    ///     Inherits from the <see cref="MemberMock" /> class.
    ///     Implements the <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MemberMock" />
    /// <seealso cref="ICanHaveNextMethodStep{TParam, TResult}" />
    public abstract class MethodMockBase<TParam, TResult> : MemberMock, ICanHaveNextMethodStep<TParam, TResult>
    {
        private IMethodStep<TParam, TResult>? _nextStep;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MethodMockBase{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock with behaviour.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        protected internal MethodMockBase(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
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
        TStep ICanHaveNextMethodStep<TParam, TResult>.SetNextStep<TStep>(TStep step)
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
        ///     Calls the mocked method.
        /// </summary>
        /// <remarks>
        ///     This method is called when the method is called through a mocked interface, but can also be used to interact with
        ///     the mock directly.
        /// </remarks>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        protected TResult Call(TParam param)
        {
            if (_nextStep == null)
            {
                IMockInfo mockInfo = this;

                if (mockInfo.Strictness == Strictness.Lenient)
                {
                    return default!;
                }

                throw new MockMissingException(MockType.Method, mockInfo);
            }

            return _nextStep.Call(this, param);
        }
    }
}
