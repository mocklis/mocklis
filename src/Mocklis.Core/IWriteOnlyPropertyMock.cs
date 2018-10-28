// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IWriteOnlyPropertyMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IWriteOnlyPropertyMock<TValue>
    {
        IPropertyImplementation<TValue> Implementation { get; }
        IWriteOnlyPropertyMock<TValue> Use(IPropertyImplementation<TValue> implementation);
    }
}
