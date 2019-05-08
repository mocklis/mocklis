// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexerMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that represents a mock of an indexer of a given type. This class cannot be inherited.
    ///     Inherits from the <see cref="MemberMock" /> class.
    ///     Implements the <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> interface.
    /// </summary>
    /// <typeparam name="TKey">The type of the indexer key.</typeparam>
    /// <typeparam name="TValue">The type of the indexer value.</typeparam>
    /// <seealso cref="MemberMock" />
    /// <seealso cref="ICanHaveNextIndexerStep{TKey, TValue}" />
    public sealed class IndexerMock<TKey, TValue> : MemberMock, ICanHaveNextIndexerStep<TKey, TValue>
    {
        private IIndexerStep<TKey, TValue> _nextStep;

        /// <summary>
        ///     Initializes a new instance of the <see cref="IndexerMock{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock with behaviour.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        public IndexerMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
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
        TStep ICanHaveNextIndexerStep<TKey, TValue>.SetNextStep<TStep>(TStep step)
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
        ///     Gets or sets the <typeparamref name="TValue" /> with the specified key on the mocked indexer.
        /// </summary>
        /// <param name="key">The indexer key used.</param>
        /// <returns>The value being read or written.</returns>
        public TValue this[TKey key]
        {
            get
            {
                if (_nextStep == null)
                {
                    IMockInfo mockInfo = this;

                    if (mockInfo.Strictness == Strictness.Lenient)
                    {
                        return default;
                    }

                    throw new MockMissingException(MockType.IndexerGet, mockInfo);
                }

                return _nextStep.Get(this, key);
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

                    throw new MockMissingException(MockType.IndexerSet, mockInfo);
                }

                _nextStep.Set(this, key, value);
            }
        }
    }
}
