// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InstanceRecordBeforeCallMethodStep.cs">
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
    ///     This class represents a 'Record' step that records when a method is about to be called.
    ///     Inherits from the <see cref="RecordMethodStepBase{TParam, TResult, TRecord}" /> class.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <typeparam name="TRecord">The type of data recorded in the ledger.</typeparam>
    /// <seealso cref="RecordMethodStepBase{TParam, TResult, TRecord}" />
    public class InstanceRecordBeforeCallMethodStep<TParam, TResult, TRecord> : RecordMethodStepBase<TParam, TResult, TRecord>
    {
        private readonly Func<object, TParam, TRecord> _selector;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InstanceRecordBeforeCallMethodStep{TParam, TResult, TRecord}" />
        ///     class.
        /// </summary>
        /// <param name="selector">
        ///     A Func that constructs an entry for when a call is made.
        ///     Takes the mocked instance and the parameters sent as parameters.
        /// </param>
        public InstanceRecordBeforeCallMethodStep(Func<object, TParam, TRecord> selector)
        {
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation adds a record to the ledger when the method is about to be called.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            Add(_selector(mockInfo.MockInstance, param));
            return base.Call(mockInfo, param);
        }
    }
}
