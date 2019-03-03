// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateStepExtensions.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.Threading;
    using System.Threading.Tasks;
    using Mocklis.Core;
    using Mocklis.Steps.Gate;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'gate' steps to an existing mock or step.
    /// </summary>
    public static class GateStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will complete a task when a method has been called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'gate' step is added.</param>
        /// <param name="task">Returns a reference to the task.</param>
        /// <param name="cancellationToken">An optional cancellation token.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextMethodStep<TParam, TResult> Gate<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            out Task<TResult> task,
            CancellationToken cancellationToken = default)
        {
            var newStep = new GateMethodStep<TParam, TResult>(cancellationToken);
            task = newStep.Task;
            return caller.SetNextStep(newStep);
        }
    }
}
