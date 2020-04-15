// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IIndexers.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    #region Using Directives

    using System;

    #endregion

    public interface IIndexers
    {
        string this[int index] { get; set; }
        DateTime this[bool index] { get; set; }
    }
}
