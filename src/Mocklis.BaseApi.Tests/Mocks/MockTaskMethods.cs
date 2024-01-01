// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockTaskMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
    #region Using Directives

    using System.CodeDom.Compiler;
    using System.Threading.Tasks;
    using Mocklis.Core;
    using Mocklis.Interfaces;

    #endregion

    [MocklisClass] [GeneratedCode("Mocklis", "1.2.0")]
    public class MockTaskMethods : ITaskMethods
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockTaskMethods()
        {
            ReturnTaskInt =
                new FuncMethodMock<Task<int>>(this, "MockTaskMethods", "ITaskMethods", "ReturnTaskInt", "ReturnTaskInt", Strictness.Lenient);
            ReturnTask = new FuncMethodMock<Task>(this, "MockTaskMethods", "ITaskMethods", "ReturnTask", "ReturnTask", Strictness.Lenient);
        }

        public FuncMethodMock<Task<int>> ReturnTaskInt { get; }

        Task<int> ITaskMethods.ReturnTaskInt() => ReturnTaskInt.Call();

        public FuncMethodMock<Task> ReturnTask { get; }

        Task ITaskMethods.ReturnTask() => ReturnTask.Call();
    }
}
