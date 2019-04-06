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
        string StringProperty { get; set; }
        int IntProperty { get; set; }
        bool BoolProperty { get; set; }
    }
}
