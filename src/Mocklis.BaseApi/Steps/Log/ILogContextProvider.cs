// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogContextProvider.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Steps.Log
{
    /// <summary>
    ///     Interface that lets a Log step obtain an <see cref="ILogContext" /> indirectly from another object,
    ///     usually a test class.
    /// </summary>
    /// <remarks>
    ///     A very common way to use this interface is to have it implemented by the test class itself, allowing
    ///     you to simply pass 'this' to any Log steps you set up in your tests.
    /// </remarks>
    public interface ILogContextProvider
    {
        /// <summary>
        ///     Gets an <see cref="ILogContext" /> for use by a Log step.
        /// </summary>
        ILogContext LogContext { get; }
    }
}
