// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuncMethodMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Class that represents a mock of a method that takes parameters and returns a result. This class cannot be
    ///     inherited.
    ///     Inherits from the <see cref="Mocklis.Core.MethodMockBase{TParam, TResult}" /> class.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="Mocklis.Core.MethodMockBase{TParam, TResult}" />
    public sealed class FuncMethodMock<TParam, TResult> : MethodMockBase<TParam, TResult>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FuncMethodMock{TParam, TResult}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mocked member is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked member is defined.</param>
        /// <param name="memberName">The name of the mocked member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mocked member with behaviour.</param>
        public FuncMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
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
        /// <returns>The returned result.</returns>
        public new TResult Call(TParam param)
        {
            return base.Call(param);
        }
    }

    /// <summary>
    ///     Class that represents a mock of a method that doesn't take parameters but returns a result. This class cannot be
    ///     inherited.
    ///     Inherits from the <see cref="Mocklis.Core.MethodMockBase{ValueTuple, TResult}" /> class.
    /// </summary>
    /// <typeparam name="TResult">The method return type.</typeparam>
    /// <seealso cref="Mocklis.Core.MethodMockBase{ValueTuple, TResult}" />
    public sealed class FuncMethodMock<TResult> : MethodMockBase<ValueTuple, TResult>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FuncMethodMock{TResult}" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mocked member is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked member is defined.</param>
        /// <param name="memberName">The name of the mocked member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mocked member with behaviour.</param>
        public FuncMethodMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
            : base(mockInstance, mocklisClassName, interfaceName, memberName, memberMockName)
        {
        }

        /// <summary>
        ///     Calls the mocked method.
        /// </summary>
        /// <remarks>
        ///     This method is called when the method is called through a mocked interface, but can also be used to interact with
        ///     the mock directly.
        /// </remarks>
        /// <returns>The returned result.</returns>
        public TResult Call()
        {
            return base.Call(ValueTuple.Create());
        }
    }
}
