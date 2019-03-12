// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalStepExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;
    using Mocklis.Steps.Conditional;

    #endregion

    /// <summary>
    ///     A class with extension methods for adding 'conditional' steps to an existing mock or step.
    /// </summary>
    public static class ConditionalStepExtensions
    {
        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided conditions.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="addCondition">
        ///     A condition evaluated when an event handler is added. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="removeCondition">
        ///     A condition evaluated when an event handler is removed. If <c>true</c>, the alternative
        ///     branch is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextEventStep<THandler> If<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<THandler, bool> addCondition,
            Func<THandler, bool> removeCondition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfEventStep<THandler>(addCondition, removeCondition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when an event handler is added or removed. If <c>true</c>, the
        ///     alternative branch is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextEventStep<THandler> If<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<THandler, bool> condition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfEventStep<THandler>(condition, condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided conditions, where the conditions can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="addCondition">
        ///     A condition evaluated when an event handler is added. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="removeCondition">
        ///     A condition evaluated when an event handler is removed. If <c>true</c>, the alternative
        ///     branch is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceIf<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<object, THandler, bool> addCondition,
            Func<object, THandler, bool> removeCondition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new InstanceIfEventStep<THandler>(addCondition, removeCondition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition, where the condition can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when an event handler is added or removed. If <c>true</c>, the
        ///     alternative branch is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextEventStep<THandler> InstanceIf<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Func<object, THandler, bool> condition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new InstanceIfEventStep<THandler>(condition, condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that is chosen when an event handler is added.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextEventStep<THandler> IfAdd<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfAddEventStep<THandler>(branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that is chosen when an event handler is removed.
        /// </summary>
        /// <typeparam name="THandler">The event handler type for the event.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextEventStep{THandler}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextEventStep<THandler> IfRemove<THandler>(
            this ICanHaveNextEventStep<THandler> caller,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfRemoveEventStep<THandler>(branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided conditions.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="getCondition">
        ///     A condition evaluated when the indexer is read from. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="setCondition">
        ///     A condition evaluated when the indexer is written to. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps on the normal
        ///     branch.
        /// </returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> If<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<TKey, bool> getCondition,
            Func<TKey, TValue, bool> setCondition,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfIndexerStep<TKey, TValue>(getCondition, setCondition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided conditions, where the conditions can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="getCondition">
        ///     A condition evaluated when the indexer is read from. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="setCondition">
        ///     A condition evaluated when the indexer is written to. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps on the normal
        ///     branch.
        /// </returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> InstanceIf<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Func<object, TKey, bool> getCondition,
            Func<object, TKey, TValue, bool> setCondition,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfIndexerStep<TKey, TValue>(getCondition, setCondition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that is chosen when the indexer is read from.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps on the normal
        ///     branch.
        /// </returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> IfGet<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfGetIndexerStep<TKey, TValue>(branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that is chosen when the indexer is written to.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps on the normal
        ///     branch.
        /// </returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> IfSet<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfSetIndexerStep<TKey, TValue>(branch));
        }

        /// <summary>
        ///     Introduces a filter that will only progress to writing a value if it's different from the current value.
        /// </summary>
        /// <typeparam name="TKey">The type of the indexer key.</typeparam>
        /// <typeparam name="TValue">The type of the indexer value.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="comparer">
        ///     An optional <see cref="IEqualityComparer{TValue}" /> that is used to determine whether the new
        ///     value is different from the current one.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextIndexerStep{TKey, TValue}" /> that can be used to add further steps.</returns>
        public static ICanHaveNextIndexerStep<TKey, TValue> OnlySetIfChanged<TKey, TValue>(
            this ICanHaveNextIndexerStep<TKey, TValue> caller,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller.SetNextStep(new OnlySetIfChangedIndexerStep<TKey, TValue>(comparer));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition.
        /// </summary>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<ValueTuple, ValueTuple> If(
            this ICanHaveNextMethodStep<ValueTuple, ValueTuple> caller,
            Func<bool> condition,
            Action<IfMethodStepBase<ValueTuple, ValueTuple>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfMethodStep<ValueTuple>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<ValueTuple, TResult> If<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<bool> condition,
            Action<IfMethodStepBase<ValueTuple, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfMethodStep<TResult>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<TParam, ValueTuple> If<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTuple> caller,
            Func<TParam, bool> condition,
            Action<IfMethodStepBase<TParam, ValueTuple>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfMethodStep<TParam, ValueTuple>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<TParam, TResult> If<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<TParam, bool> condition,
            Action<IfMethodStepBase<TParam, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfMethodStep<TParam, TResult>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition, where the condition can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<ValueTuple, ValueTuple> InstanceIf(
            this ICanHaveNextMethodStep<ValueTuple, ValueTuple> caller,
            Func<object, bool> condition,
            Action<IfMethodStepBase<ValueTuple, ValueTuple>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfMethodStep<ValueTuple>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition, where the condition can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<ValueTuple, TResult> InstanceIf<TResult>(
            this ICanHaveNextMethodStep<ValueTuple, TResult> caller,
            Func<object, bool> condition,
            Action<IfMethodStepBase<ValueTuple, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfMethodStep<TResult>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition, where the condition can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<TParam, ValueTuple> InstanceIf<TParam>(
            this ICanHaveNextMethodStep<TParam, ValueTuple> caller,
            Func<object, TParam, bool> condition,
            Action<IfMethodStepBase<TParam, ValueTuple>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfMethodStep<TParam, ValueTuple>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided condition, where the condition can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="TParam">The method parameter type.</typeparam>
        /// <typeparam name="TResult">The method return type.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="condition">
        ///     A condition evaluated when the method is called. If <c>true</c>, the alternative branch is
        ///     taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>
        ///     An <see cref="ICanHaveNextMethodStep{ValueTuple, TResult}" /> that can be used to add further steps on the
        ///     normal branch.
        /// </returns>
        public static ICanHaveNextMethodStep<TParam, TResult> InstanceIf<TParam, TResult>(
            this ICanHaveNextMethodStep<TParam, TResult> caller,
            Func<object, TParam, bool> condition,
            Action<IfMethodStepBase<TParam, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfMethodStep<TParam, TResult>(condition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided conditions.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="getCondition">
        ///     A condition evaluated when the indexer is read from. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="setCondition">
        ///     A condition evaluated when the indexer is written to. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextPropertyStep<TValue> If<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<bool> getCondition,
            Func<TValue, bool> setCondition,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfPropertyStep<TValue>(getCondition, setCondition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that can be chosen given the provided conditions, where the conditions can
        ///     depend on the state of the entire mock instance.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="getCondition">
        ///     A condition evaluated when the indexer is read from. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="setCondition">
        ///     A condition evaluated when the indexer is written to. If <c>true</c>, the alternative branch
        ///     is taken.
        /// </param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextPropertyStep<TValue> InstanceIf<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Func<object, bool> getCondition,
            Func<object, TValue, bool> setCondition,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfPropertyStep<TValue>(getCondition, setCondition, branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that is chosen when the property is read from.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextPropertyStep<TValue> IfGet<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfGetPropertyStep<TValue>(branch));
        }

        /// <summary>
        ///     Introduces an alternative set of steps that is chosen when the property is written to.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="branch">
        ///     An action to set up the alternative branch; it also provides a means of re-joining the normal
        ///     branch.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextPropertyStep<TValue> IfSet<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfSetPropertyStep<TValue>(branch));
        }

        /// <summary>
        ///     Introduces a filter that will only progress to writing a value if it's different from the current value.
        /// </summary>
        /// <typeparam name="TValue">The type of the property.</typeparam>
        /// <param name="caller">The mock or step to which this 'conditional' step is added.</param>
        /// <param name="comparer">
        ///     An optional <see cref="IEqualityComparer{TValue}" /> that is used to determine whether the new
        ///     value is different from the current one.
        /// </param>
        /// <returns>An <see cref="ICanHaveNextPropertyStep{TValue}" /> that can be used to add further steps on the normal branch.</returns>
        public static ICanHaveNextPropertyStep<TValue> OnlySetIfChanged<TValue>(
            this ICanHaveNextPropertyStep<TValue> caller,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller.SetNextStep(new OnlySetIfChangedPropertyStep<TValue>(comparer));
        }
    }
}
