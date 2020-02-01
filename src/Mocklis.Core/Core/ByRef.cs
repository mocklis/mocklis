// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ByRef.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Class that wraps a value so that it can be return by reference. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">The type of the wrapped value.</typeparam>
    public sealed class ByRef<T>
    {
        /// <summary>
        ///     Wraps the specified value so that it can be returned by reference.
        /// </summary>
        /// <param name="value">The value to be wrapped.</param>
        /// <returns>A reference to the value.</returns>
        public static ref T Wrap(T value)
        {
            return ref new ByRef<T>(value).Value;
        }

        private T _value;

        private ByRef(T value)
        {
            _value = value;
        }

        private ref T Value => ref _value;
    }
}
