// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMockInfo.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that provides information about a mock member that is accessed.
    /// </summary>
    public interface IMockInfo
    {
        /// <summary>
        ///     Gets the instance of the mocklis class through with the mock is accessed.
        /// </summary>
        /// <value>The instance of the mocklis class through with the mock is accessed.</value>
        object MockInstance { get; }

        /// <summary>
        ///     Gets the name of the mocklis class.
        /// </summary>
        /// <value>The name of the mocklis class.</value>
        string MocklisClassName { get; }

        /// <summary>
        ///     Gets the name of the interface on which the mocked member is defined.
        /// </summary>
        /// <value>The name of the interface on which the mocked member is defined.</value>
        string InterfaceName { get; }

        /// <summary>
        ///     Gets the name of the mocked interface member.
        /// </summary>
        /// <value>The name of the mocked interface member.</value>
        string MemberName { get; }

        /// <summary>
        ///     Gets the name of the property or method used to provide the mock with behaviour.
        /// </summary>
        /// <value>The name of the property or method used to provide the mock with behaviour.</value>
        string MemberMockName { get; }

        /// <summary>
        ///     Gets the strictness of the mock.
        /// </summary>
        /// <value>The strictness of the mock.</value>
        Strictness Strictness { get; }
    }
}
