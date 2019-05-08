// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Strictness.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     A value denoting to what extent unconfigured mocks should throw exceptions (strict) or return default values.
    /// </summary>
    public enum Strictness
    {
        /// <summary>
        ///     A strictness value indicating that a default value should be returned from unconfigured mocks and steps.
        /// </summary>
        Lenient = 0,

        /// <summary>
        ///     A strictness value indicating that an unconfigured mock should throw a <see cref="MockMissingException" />,
        ///     but that a step should return a default value in lieu of an unconfigured 'next' step.
        /// </summary>
        Strict = 1,

        /// <summary>
        ///     A strictness value indicating that an unconfigured mock or step should both throw a
        ///     <see cref="MockMissingException" />.
        /// </summary>
        VeryStrict = 2
    }
}
