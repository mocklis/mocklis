// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventStep.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Interface that models things that can happen with a mocked event.
    /// </summary>
    /// <typeparam name="THandler">The event handler type for the event.</typeparam>
    public interface IEventStep<in THandler> where THandler : Delegate
    {
        /// <summary>
        ///     Called when an event handler is being added to the mocked event.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being added.</param>
        /// <param name="value">The event handler that is being added.</param>
        void Add(IMockInfo mockInfo, THandler value);

        /// <summary>
        ///     Called when an event handler is being removed to the mocked event.
        /// </summary>
        /// <param name="mockInfo">Information about the mock through which the event handler is being removed.</param>
        /// <param name="value">The event handler that is being removed.</param>
        void Remove(IMockInfo mockInfo, THandler value);
    }
}
