// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    #region Using Directives

    #endregion

    public interface IMethods
    {
        void SimpleAction();
        void ActionWithParameter(int i);
        int SimpleFunc();
        int FuncWithParameter(int i);
    }
}
