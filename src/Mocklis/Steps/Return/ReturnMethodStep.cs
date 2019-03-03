// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReturnMethodStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Return
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'Return' method step that returns a given result every time it's called.
    ///     Implements the <see cref="MethodStepWithNext{TParam,TResult}" />
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="MethodStepWithNext{TParam, TResult}" />
    public class ReturnMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private readonly TResult _result;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReturnMethodStep{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="result">The result to be returned.</param>
        public ReturnMethodStep(TResult result)
        {
            _result = result;
        }

        /// <summary>
        ///     Called when the mocked method is called.
        ///     This implementation returns a given result every time.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            return _result;
        }
    }
}
