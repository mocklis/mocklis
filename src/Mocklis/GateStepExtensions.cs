// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GateStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System.Threading;
    using Mocklis.Core;
    using Mocklis.Gate;

    #endregion

    public static class GateStepExtensions
    {
        public static IMethodStepCaller<TParam, TResult> Gate<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            out IGate gate,
            CancellationToken cancellationToken = default)
        {
            var newStep = new GateMethodStep<TParam, TResult>(cancellationToken);
            gate = newStep;
            return caller.SetNextStep(newStep);
        }
    }
}
