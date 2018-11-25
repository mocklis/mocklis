// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IPropertyStep<TValue>
    {
        TValue Get(IMockInfo mockInfo);
        void Set(IMockInfo mockInfo, TValue value);
    }
}
