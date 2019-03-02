// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpectedUsageMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Steps
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Method step that counts the number of times the method has been called, and can verify this against a given
    ///     expected number of calls. This class cannot be inherited.
    ///     Inherits from the <see cref="MethodStepWithNext{TParam,TResult}" /> class.
    ///     Implements the <see cref="IVerifiable" /> interface.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    /// <seealso cref="IVerifiable" />
    public sealed class ExpectedUsageMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>, IVerifiable
    {
        private readonly string _name;
        private readonly int _expectedNumberOfCalls;
        private int _currentNumberOfCalls;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpectedUsageMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="name">The name of the verification.</param>
        /// <param name="expectedNumberOfCalls">The expected number of calls.</param>
        public ExpectedUsageMethodStep(string name, int expectedNumberOfCalls)
        {
            _name = name;
            _expectedNumberOfCalls = expectedNumberOfCalls;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     Increases a counter that keeps track of the number of times the method has been called.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            Interlocked.Increment(ref _currentNumberOfCalls);
            return base.Call(mockInfo, param);
        }

        /// <summary>
        ///     Verifies that the method has been called the expected number of times.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.Generic.IEnumerable`1" /> with information about the verifications and
        ///     whether they were successful.
        /// </returns>
        public IEnumerable<VerificationResult> Verify()
        {
            string prefix = string.IsNullOrEmpty(_name) ? "Usage Count" : $"Usage Count '{_name}'";
            yield return new VerificationResult($"{prefix}: Expected {_expectedNumberOfCalls} call(s); received {_currentNumberOfCalls} call(s).",
                _expectedNumberOfCalls == _currentNumberOfCalls);
        }
    }
}
