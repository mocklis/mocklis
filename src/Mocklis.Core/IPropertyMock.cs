// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IPropertyMock<TValue>
    {
        IPropertyImplementation<TValue> Implementation { get; }
        IPropertyMock<TValue> Use(IPropertyImplementation<TValue> implementation);
    }
}
