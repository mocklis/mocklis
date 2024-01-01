// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Interfaces
{
    public interface IMethods
    {
        void SimpleAction();
        void ActionWithParameter(int i);
        int SimpleFunc();
        int FuncWithParameter(int i);
    }
}
