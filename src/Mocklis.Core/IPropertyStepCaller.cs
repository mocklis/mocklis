// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStepCaller.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IPropertyStepCaller<TValue>
    {
        IPropertyStep<TValue> NextStep { get; }
        TStep SetNextStep<TStep>(TStep step) where TStep : IPropertyStep<TValue>;
    }
}
