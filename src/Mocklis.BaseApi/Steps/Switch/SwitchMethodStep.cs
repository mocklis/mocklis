// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SwitchMethodStep.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Switch
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Mocklis.Core;

    #endregion

    /// <summary>
    ///     Class that represents a 'switch' method step that allows for setting up different branches that can be taken
    ///     based on the input parameters.
    /// </summary>
    /// <typeparam name="TParam">The method parameter type.</typeparam>
    /// <typeparam name="TResult">The method return type.</typeparam>
    public sealed class SwitchMethodStep<TParam, TResult> : IMethodStep<TParam, TResult>
    {
        private sealed class SwitchTableEntry
        {
            public Predicate<TParam> Predicate { get; }
            public MethodStepWithNext<TParam, TResult> NextStep { get; }

            public SwitchTableEntry(Predicate<TParam> predicate)
            {
                Predicate = predicate;
                NextStep = new MethodStepWithNext<TParam, TResult>();
            }
        }

        private readonly object _lockObject = new object();

        private readonly bool _consumeOnUse;

        private readonly List<SwitchTableEntry> _switchTable = new List<SwitchTableEntry>();

        private readonly MethodStepWithNext<TParam, TResult> _otherwiseStep = new MethodStepWithNext<TParam, TResult>();

        /// <summary>
        /// Initialises a new instance of the <see cref="SwitchMethodStep{TParam,TResult}"/> class.
        /// </summary>
        /// <param name="consumeOnUse">If <c>true</c> this parameter makes the mock forget the switch branches as they are used. Defaults to <c>false</c>.</param>
        public SwitchMethodStep(bool consumeOnUse)
        {
            _consumeOnUse = consumeOnUse;
        }

        /// <summary>
        ///     Called when the mocked method is called. This implementation picks the first acceptable switch branch, or the default one if none was found.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the method is called.</param>
        /// <param name="param">The parameters used.</param>
        /// <returns>The returned result.</returns>
        public TResult Call(IMockInfo mockInfo, TParam param)
        {
            MethodStepWithNext<TParam, TResult> FindNextStep()
            {
                lock (_lockObject)
                {
                    for (int i = 0; i < _switchTable.Count; i++)
                    {
                        var entry = _switchTable[i];
                        if (entry.Predicate(param))
                        {
                            if (_consumeOnUse)
                            {
                                _switchTable.RemoveAt(i);
                            }

                            return entry.NextStep;
                        }
                    }

                    return _otherwiseStep;
                }
            }

            return FindNextStep().Call(mockInfo, param);
        }

        /// <summary>
        ///     Adds a new branch to the switch. Takes a predicate (a lambda that returns <c>true</c> if the branch is to be taken), and returns
        ///     a programmable mock for the branch.
        /// </summary>
        /// <param name="predicate">A lambda that is used to determine if the new branch is to be taken.</param>
        /// <returns>An <see cref="ICanHaveNextMethodStep{TParam, TResult}" /> that can be used to provide functionality to the branch.</returns>
        public ICanHaveNextMethodStep<TParam, TResult> When(Predicate<TParam> predicate)
        {
            var entry = new SwitchTableEntry(predicate);
            lock (_lockObject)
            {
                _switchTable.Add(entry);
            }

            return entry.NextStep;
        }

        /// <summary>
        ///     A property for the 'default' branch of the switch, that is to say what will happen if none of the branches added with the <c>When</c> method
        ///     matches the parameters passed in the call.
        /// </summary>
        public ICanHaveNextMethodStep<TParam, TResult> Otherwise => _otherwiseStep;

        /// <summary>
        ///     Clears all mock behaviour set up for this switch.
        /// </summary>
        public void Clear()
        {
            lock (_lockObject)
            {
                _switchTable.Clear();
                _otherwiseStep.Clear();
            }
        }
    }
}
