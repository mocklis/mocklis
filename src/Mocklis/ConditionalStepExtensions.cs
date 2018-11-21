// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConditionalStepExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
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

    public static class ConditionalStepExtensions
    {
        public static IEventStepCaller<THandler> If<THandler>(
            this IEventStepCaller<THandler> caller,
            Func<THandler, bool> addCondition,
            Func<THandler, bool> removeCondition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfEventStep<THandler>(addCondition, removeCondition, branch));
        }

        public static IEventStepCaller<THandler> If<THandler>(
            this IEventStepCaller<THandler> caller,
            Func<THandler, bool> condition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfEventStep<THandler>(condition, condition, branch));
        }

        public static IEventStepCaller<THandler> InstanceIf<THandler>(
            this IEventStepCaller<THandler> caller,
            Func<object, THandler, bool> addCondition,
            Func<object, THandler, bool> removeCondition,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new InstanceIfEventStep<THandler>(addCondition, removeCondition, branch));
        }

        public static IEventStepCaller<THandler> IfAdd<THandler>(
            this IEventStepCaller<THandler> caller,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfAddEventStep<THandler>(branch));
        }

        public static IEventStepCaller<THandler> IfRemove<THandler>(
            this IEventStepCaller<THandler> caller,
            Action<IfEventStepBase<THandler>.IfBranchCaller> branch) where THandler : Delegate
        {
            return caller.SetNextStep(new IfRemoveEventStep<THandler>(branch));
        }

        public static IIndexerStepCaller<TKey, TValue> If<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<TKey, bool> getCondition,
            Func<TKey, TValue, bool> setCondition,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfIndexerStep<TKey, TValue>(getCondition, setCondition, branch));
        }

        public static IIndexerStepCaller<TKey, TValue> InstanceIf<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Func<object, TKey, bool> getCondition,
            Func<object, TKey, TValue, bool> setCondition,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfIndexerStep<TKey, TValue>(getCondition, setCondition, branch));
        }

        public static IIndexerStepCaller<TKey, TValue> IfGet<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfGetIndexerStep<TKey, TValue>(branch));
        }

        public static IIndexerStepCaller<TKey, TValue> IfSet<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            Action<IfIndexerStepBase<TKey, TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfSetIndexerStep<TKey, TValue>(branch));
        }

        public static IIndexerStepCaller<TKey, TValue> OnlySetIfChanged<TKey, TValue>(
            this IIndexerStepCaller<TKey, TValue> caller,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller.SetNextStep(new OnlySetIfChangedIndexerStep<TKey, TValue>(comparer));
        }

        public static IMethodStepCaller<ValueTuple, TResult> If<TResult>(
            this IMethodStepCaller<ValueTuple, TResult> caller,
            Func<bool> condition,
            Action<IfMethodStepBase<ValueTuple, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfMethodStep<TResult>(condition, branch));
        }

        public static IMethodStepCaller<TParam, TResult> If<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<TParam, bool> condition,
            Action<IfMethodStepBase<TParam, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfMethodStep<TParam, TResult>(condition, branch));
        }

        public static IMethodStepCaller<ValueTuple, TResult> InstanceIf<TResult>(
            this IMethodStepCaller<ValueTuple, TResult> caller,
            Func<object, bool> condition,
            Action<IfMethodStepBase<ValueTuple, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfMethodStep<TResult>(condition, branch));
        }

        public static IMethodStepCaller<TParam, TResult> InstanceIf<TParam, TResult>(
            this IMethodStepCaller<TParam, TResult> caller,
            Func<object, TParam, bool> condition,
            Action<IfMethodStepBase<TParam, TResult>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfMethodStep<TParam, TResult>(condition, branch));
        }

        public static IPropertyStepCaller<TValue> If<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Func<bool> getCondition,
            Func<TValue, bool> setCondition,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfPropertyStep<TValue>(getCondition, setCondition, branch));
        }

        public static IPropertyStepCaller<TValue> InstanceIf<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Func<object, bool> getCondition,
            Func<object, TValue, bool> setCondition,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new InstanceIfPropertyStep<TValue>(getCondition, setCondition, branch));
        }

        public static IPropertyStepCaller<TValue> IfGet<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfGetPropertyStep<TValue>(branch));
        }

        public static IPropertyStepCaller<TValue> IfSet<TValue>(
            this IPropertyStepCaller<TValue> caller,
            Action<IfPropertyStepBase<TValue>.IfBranchCaller> branch)
        {
            return caller.SetNextStep(new IfSetPropertyStep<TValue>(branch));
        }

        public static IPropertyStepCaller<TValue> OnlySetIfChanged<TValue>(
            this IPropertyStepCaller<TValue> caller,
            IEqualityComparer<TValue> comparer = null)
        {
            return caller.SetNextStep(new OnlySetIfChangedPropertyStep<TValue>(comparer));
        }
    }
}
