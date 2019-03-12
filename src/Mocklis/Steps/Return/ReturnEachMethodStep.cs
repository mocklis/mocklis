// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnEachMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' method step that returns a given set of results one-by-one, and then forwards on
    ///     calls.
    ///     Implements the <see cref="MethodStepWithNext{TParam,TResult}" />
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public class ReturnEachMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>
    {
        private readonly object _lockObject = new object();
        private IEnumerator<TResult> _results;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnEachMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="results">The values to be returned one-by-one.</param>
        public ReturnEachMethodStep(IEnumerable<TResult> results)
        {
            _results = results?.GetEnumerator();
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation returns the results provided one-by-one, and then forwards on calls.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            if (_results == null)
            {
                return base.Call(mockInfo, param);
            }

            lock (_lockObject)
            {
                if (_results != null)
                {
                    if (_results.MoveNext())
                    {
                        return _results.Current;
                    }

                    _results.Dispose();
                    _results = null;
                }
            }

            return base.Call(mockInfo, param);
        }
    }
}
