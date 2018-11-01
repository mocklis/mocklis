// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethodStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IMethodStepCaller<TParam, TResult>
    {
        IMethodStep<TParam, TResult> NextStep { get; }
        TStep SetNextStep<TStep>(TStep step) where TStep : IMethodStep<TParam, TResult>;
    }
}
