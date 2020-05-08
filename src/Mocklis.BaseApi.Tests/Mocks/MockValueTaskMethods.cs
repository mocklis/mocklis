// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockValueTaskMethods.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Mocks
{
#if NETCOREAPP3_0

    #region Using Directives

    using System.Threading.Tasks;
    using Mocklis.Core;
    using Mocklis.Interfaces;

    #endregion

    [MocklisClass] [System.CodeDom.Compiler.GeneratedCode("Mocklis", "1.2.0")]
    public class MockValueTaskMethods : IValueTaskMethods
    {
        // The contents of this class were created by the Mocklis code-generator.
        // Any changes you make will be overwritten if the contents are re-generated.

        public MockValueTaskMethods()
        {
            ReturnValueTaskInt = new FuncMethodMock<ValueTask<int>>(this, "MockValueTaskMethods", "IValueTaskMethods", "ReturnValueTaskInt",
                "ReturnValueTaskInt", Strictness.Lenient);
            ReturnValueTask = new FuncMethodMock<ValueTask>(this, "MockValueTaskMethods", "IValueTaskMethods", "ReturnValueTask", "ReturnValueTask",
                Strictness.Lenient);
        }

        public FuncMethodMock<ValueTask<int>> ReturnValueTaskInt { get; }

        ValueTask<int> IValueTaskMethods.ReturnValueTaskInt() => ReturnValueTaskInt.Call();

        public FuncMethodMock<ValueTask> ReturnValueTask { get; }

        ValueTask IValueTaskMethods.ReturnValueTask() => ReturnValueTask.Call();
    }

#endif

}
