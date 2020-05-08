// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITaskMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    #region Using Directives

    using System.Threading.Tasks;

    #endregion

    public interface ITaskMethods
    {
        Task<int> ReturnTaskInt();
        Task ReturnTask();
    }
}
