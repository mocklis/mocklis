// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockTaskMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using Mocklis.Core;
    using Mocklis.Interfaces;

    #endregion

    [MocklisClass] [System.CodeDom.Compiler.GeneratedCode("Mocklis", "1.2.0")]
    public class MockTaskMethods : ITaskMethods
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockTaskMethods()
        {
            ReturnTaskInt = new FuncMethodMock<System.Threading.Tasks.Task<int>>(this, "MockTaskMethods", "ITaskMethods", "ReturnTaskInt", "ReturnTaskInt", Strictness.Lenient);
            ReturnTask = new FuncMethodMock<System.Threading.Tasks.Task>(this, "MockTaskMethods", "ITaskMethods", "ReturnTask", "ReturnTask", Strictness.Lenient);
        }

        public FuncMethodMock<System.Threading.Tasks.Task<int>> ReturnTaskInt { get; }

        System.Threading.Tasks.Task<int> ITaskMethods.ReturnTaskInt() => ReturnTaskInt.Call();

        public FuncMethodMock<System.Threading.Tasks.Task> ReturnTask { get; }

        System.Threading.Tasks.Task ITaskMethods.ReturnTask() => ReturnTask.Call();
    }
}
