// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyStep.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IPropertyStep<TValue>
    {
        TValue Get(object instance, MemberMock memberMock);
        void Set(object instance, MemberMock memberMock, TValue value);
    }
}
