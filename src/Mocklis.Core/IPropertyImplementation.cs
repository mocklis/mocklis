// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyImplementation.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IPropertyImplementation<TValue>
    {
        TValue Get(MemberMock memberMock);
        void Set(MemberMock memberMock, TValue value);
    }
}
