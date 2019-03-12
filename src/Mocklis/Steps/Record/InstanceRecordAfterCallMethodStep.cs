// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordAfterCallMethodStep.cs">
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
    public class InstanceRecordAfterCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<object, TParam, TResult, TRecord> _selection;
        private readonly Func<object, Exception, TRecord> _onError;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceRecordAfterCallMethodStep{TParam, TResult, TRecord}" /> class.
        /// </summary>
        /// <param name="selection">
        ///     A Func that selects what we want to record. Takes the entire state of the mock, the parameters sent to the call
        ///     and the value returned from the call as parameters.
        /// </param>
        /// <param name="onError">
        ///     An optional Func that selects what we want to record if the call threw an exception. Takes the entire state of the
        ///     mock
        ///     and the exception as parameters.
        /// </param>
        public InstanceRecordAfterCallMethodStep(Func<object, TParam, TResult, TRecord> selection, Func<object, Exception, TRecord> onError = null)
        {
            _selection = selection ?? throw new ArgumentNullException(nameof(selection));
            _onError = onError;
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
                if (_onError != null)
                {
                    Add(_onError(mockInfo.MockInstance, exception));
                }

                throw;
            }

            Add(_selection(mockInfo.MockInstance, param, result));

            return result;
        }
    }
}
