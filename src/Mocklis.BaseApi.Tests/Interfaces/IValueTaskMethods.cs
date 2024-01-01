// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueTaskMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    #region Using Directives

    using System.Threading.Tasks;

    #endregion

    public interface IValueTaskMethods
    {
        ValueTask<int> ReturnValueTaskInt();
        ValueTask ReturnValueTask();
    }
}
