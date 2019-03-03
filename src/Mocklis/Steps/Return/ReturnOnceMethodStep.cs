// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnOnceMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' method step that returns a given result once, and then forwards on calls.
    ///     Implements the <see cref="MethodStepWithNext{TParam,TResult}" />
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public class ReturnOnceMethodStep<TParam, TResult> : MethodStepWithNext<TParam, TResult>
    {
        private readonly TResult _result;
        private int _returnCount;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnOnceMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="result">The result to return.</param>
        public ReturnOnceMethodStep(TResult result)
        {
            _result = result;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation returns the given result once, and then forwards on calls.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public override TResult Call(IMockInfo mockInfo, TParam param)
        {
            if (Interlocked.Exchange(ref _returnCount, 1) == 0)
            {
                return _result;
            }

            return base.Call(mockInfo, param);
        }
    }
}
