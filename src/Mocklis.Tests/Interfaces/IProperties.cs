// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProperties.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Interfaces
{
    #region Using Directives

    using System;

    #endregion

    public interface IProperties
    {
        string StringProperty { get; set; }
        int IntProperty { get; set; }
        bool BoolProperty { get; set; }
        DateTime DateTimeProperty { get; set; }
    }
}
