// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ActionMethodMock.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that represents a mock of a method that takes parameters but doesn't return a result. This class cannot be
    ///     inherited.
    ///     Implements the <see cref="MethodMockBase{TParam, ValueTuple}" />
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <seealso cref="MethodMockBase{TParam, ValueTuple}" />
    public sealed class ActionMethodMock<TParam> : MethodMockBase<TParam, ValueTuple>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionMethodMock{TParam}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock with behaviour.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        public ActionMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
            string memberMockName, Strictness strictness)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName, strictness)
        {
        }

        /// <summary>
        ///     Calls the mocked method.
        /// </summary>
        /// <remarks>
        ///     This method is called when the method is called through a mocked interface, but can also be used to interact with
        ///     the mock directly.
        /// </remarks>
        /// <param name="param">The parameters used.</param>
        public new void Call(TParam param)
        {
            base.Call(param);
        }
    }

    /// <summary>
    ///     Class that represents a mock of a method that doesn't take parameters and also doesn't return a result. This class
    ///     cannot be inherited.
    ///     Inherits from the <see cref="MethodMockBase{ValueTuple, ValueTuple}" /> class.
    /// </summary>
    /// <seealso cref="MethodMockBase{ValueTuple, ValueTuple}" />
    public sealed class ActionMethodMock : MethodMockBase<ValueTuple, ValueTuple>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionMethodMock" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock with behaviour.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        public ActionMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName,
            string memberMockName, Strictness strictness)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName, strictness)
        {
        }

        /// <summary>
        ///     Calls the mocked method.
        /// </summary>
        /// <remarks>
        ///     This method is called when the method is called through a mocked interface, but can also be used to interact with
        ///     the mock directly.
        /// </remarks>
        public void Call()
        {
            base.Call(ValueTuple.Create());
        }
    }
}
