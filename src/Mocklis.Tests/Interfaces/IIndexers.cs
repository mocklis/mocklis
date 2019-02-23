// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexers.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Interfaces
{
    public interface IIndexers
    {
        string this[int index] { get; set; }
    }
}
