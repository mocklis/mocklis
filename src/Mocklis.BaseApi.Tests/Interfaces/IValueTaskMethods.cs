// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IValueTaskMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
#if NETCOREAPP3_0

    #region Using Directives

    using System.Threading.Tasks;

    #endregion

    public interface IValueTaskMethods
    {
        ValueTask<int> ReturnValueTaskInt();
        ValueTask ReturnValueTask();
    }

#endif
}
