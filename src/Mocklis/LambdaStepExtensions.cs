// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LambdaStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using Mocklis.Core;
    using Mocklis.Steps.Lambda;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'lambda' steps to an existing mock or step.
    /// </summary>
    public static class LambdaStepExtensions
    {
        /// <summary>
        ///     Introduces a step that will invoke an action when an event handler is added, while forwarding
        ///     removes to a next step.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be taken when an event handler is added.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> AddAction<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Action<THandler?> action) where THandler : Delegate
        {
            return caller.SetNextStep(new AddActionEventStep<THandler>(action));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action when an event handler is removed, while forwarding
        ///     adds to a next step.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be taken when an event handler is added.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> RemoveAction<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Action<THandler?> action) where THandler : Delegate
        {
            return caller.SetNextStep(new RemoveActionEventStep<THandler>(action));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action when an event handler is added, while forwarding
        ///     removes to a next step.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be taken when an event handler is added.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceAddAction<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Action<object, THandler?> action) where THandler : Delegate
        {
            return caller.SetNextStep(new InstanceAddActionEventStep<THandler>(action));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action when an event handler is removed, while forwarding
        ///     adds to a next step.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be taken when an event handler is added.</param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceRemoveAction<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Action<object, THandler?> action) where THandler : Delegate
        {
            return caller.SetNextStep(new InstanceRemoveActionEventStep<THandler>(action));
        }

        /// <summary>
        ///     Introduces a step that will get values by calculating them from the key, while forwarding setting of values to a
        ///     next step.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the value from the key.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> GetFunc<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<TKey, TValue> func)
        {
            return caller.SetNextStep(new GetFuncIndexerStep<TKey, TValue>(func));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action whenever a value is set, while forwarding getting of values to a
        ///     next step.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be invoked when a value is set.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> SetAction<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Action<TKey, TValue> action)
        {
            return caller.SetNextStep(new SetActionIndexerStep<TKey, TValue>(action));
        }

        /// <summary>
        ///     Introduces a step that will get values by calculating them from the mock instance and key, while forwarding setting
        ///     of values to a next step.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the value from the mock instance and key.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceGetFunc<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<object, TKey, TValue> func)
        {
            return caller.SetNextStep(new InstanceGetFuncIndexerStep<TKey, TValue>(func));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action whenever a value is set, while forwarding getting of values to a
        ///     next step.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be invoked when a value is set.</param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceSetAction<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Action<object, TKey, TValue> action)
        {
            return caller.SetNextStep(new InstanceSetActionIndexerStep<TKey, TValue>(action));
        }

        /// <summary>
        ///     Introduces a step that will execute an action based on parameters when the mock method is called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be executed.</param>
        public static void Action<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTuple> caller,
            Action<TParam> action)
        {
            caller.SetNextStep(new ActionMethodStep<TParam>(action));
        }

        /// <summary>
        ///     Introduces a step that will execute an action when the mock method is called.
        /// </summary>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be executed.</param>
        public static void Action(
            this ICanHaveNextMethodStep<ValueTuple, ValueTuple> caller,
            Action action)
        {
            caller.SetNextStep(new ActionMethodStep(action));
        }

        /// <summary>
        ///     Introduces a step that will calculate a return value from parameters when the mock method is called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the return value.</param>
        public static void Func<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<TParam, TResult> func)
        {
            caller.SetNextStep(new FuncMethodStep<TParam, TResult>(func));
        }

        /// <summary>
        ///     Introduces a step that will calculate a return value when the mock method is called.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the return value.</param>
        public static void Func<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<TResult> func)
        {
            caller.SetNextStep(new FuncMethodStep<TResult>(func));
        }

        /// <summary>
        ///     Introduces a step that will execute an action based on the mock instance and parameters when the mock method is
        ///     called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be executed.</param>
        public static void InstanceAction<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTuple> caller,
            Action<object, TParam> action)
        {
            caller.SetNextStep(new InstanceActionMethodStep<TParam>(action));
        }

        /// <summary>
        ///     Introduces a step that will execute an action based on the mock instance when the mock method is called.
        /// </summary>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be executed.</param>
        public static void InstanceAction(
            this ICanHaveNextMethodStep<ValueTuple, ValueTuple> caller,
            Action<object> action)
        {
            caller.SetNextStep(new InstanceActionMethodStep(action));
        }

        /// <summary>
        ///     Introduces a step that will calculate a return value from the mock instance and parameters when the mock method is
        ///     called.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the return value.</param>
        public static void InstanceFunc<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<object, TParam, TResult> func)
        {
            caller.SetNextStep(new InstanceFuncMethodStep<TParam, TResult>(func));
        }

        /// <summary>
        ///     Introduces a step that will calculate a return value from the mock instance when the mock method is called.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the return value.</param>
        public static void InstanceFunc<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<object, TResult> func)
        {
            caller.SetNextStep(new InstanceFuncMethodStep<TResult>(func));
        }

        /// <summary>
        ///     Introduces a step that will get values by calculating them, while forwarding setting of values to a next step.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the value.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> GetFunc<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<TValue> func)
        {
            return caller.SetNextStep(new GetFuncPropertyStep<TValue>(func));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action whenever a value is set, while forwarding getting of values to a
        ///     next step.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be invoked when a value is set.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> SetAction<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Action<TValue> action)
        {
            return caller.SetNextStep(new SetActionPropertyStep<TValue>(action));
        }

        /// <summary>
        ///     Introduces a step that will get values by calculating them from the mock instance, while forwarding setting of
        ///     values to a next step.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="func">The function used to calculate the value from the mock instance.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> InstanceGetFunc<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<object, TValue> func)
        {
            return caller.SetNextStep(new InstanceGetFuncPropertyStep<TValue>(func));
        }

        /// <summary>
        ///     Introduces a step that will invoke an action whenever a value is set, while forwarding getting of values to a
        ///     next step.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'lambda' step is added.</param>
        /// <param name="action">The action to be invoked when a value is set.</param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextPropertyStep<TValue> InstanceSetAction<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Action<object, TValue> action)
        {
            return caller.SetNextStep(new InstanceSetActionPropertyStep<TValue>(action));
        }
    }
}
