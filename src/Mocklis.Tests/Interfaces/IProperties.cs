// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProperties.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Interfaces
{
    public interface IProperties
    {
        string Name { get; set; }
        int Age { get; set; }
    }
}
