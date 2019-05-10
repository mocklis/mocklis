// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordAfterCallMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Record
{
    #region Using Directives

    using System;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     This class represents a 'Record' step that records when a method has been called.
    ///     Inherits from the <see cref="RecordMethodStepBase{TParam, TResult, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordMethodStepBase{TParam, TResult, TRecord}" />
    public class RecordAfterCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<TParam, TResult, TRecord> _successSelector;
        private readonly Func<TParam, Exception, TRecord> _failureSelector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RecordAfterCallMethodStep{TParam, TResult, TRecord}" /> class.
        /// </summary>
        /// <param name="successSelector">
        ///     A Func that constructs an entry for when a result is returned from a call.
        ///     Takes the parameters sent and the returned value as parameters.
        /// </param>
        /// <param name="failureSelector">
        ///     A Func that constructs an entry for an exception thrown by a call.
        ///     Takes the parameters sent and the exception as parameters.
        /// </param>
        public RecordAfterCallMethodStep(Func<TParam, TResult, TRecord> successSelector, Func<TParam, Exception, TRecord> failureSelector)
        {
            if (successSelector == null && failureSelector == null)
            {
                throw new ArgumentException(@"The successSelector is mandatory if the FailureSelector is null.", nameof(successSelector));
            }

            _successSelector = successSelector;
            _failureSelector = failureSelector;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation records the result of the call (be it value or exception) in the ledger once the call returns.
        /// </summary>
        /// <remarks>Exceptions are only recorded if the step was given an 'onError' Func.</remarks>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            TResult result;
            try
            {
                result = base.Call(mockInfo, param);
            }
            catch (Exception exception)
            {
                if (_failureSelector != null)
                {
                    Add(_failureSelector(param, exception));
                }

                throw;
            }

            if (_successSelector != null)
            {
                Add(_successSelector(param, result));
            }

            return result;
        }
    }
}
