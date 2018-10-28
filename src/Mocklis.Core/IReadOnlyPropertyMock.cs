// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReadOnlyPropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IReadOnlyPropertyMock<TValue>
    {
        IPropertyImplementation<TValue> Implementation { get; }
        IReadOnlyPropertyMock<TValue> Use(IPropertyImplementation<TValue> implementation);
    }
}
