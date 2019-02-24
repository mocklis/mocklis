// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMockInfo.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
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
        ///     Gets the instance of the mocklis class through with the mocked member is accessed.
        /// </summary>
        /// <value>The instance of the mocklis class through with the mocked member is accessed.</value>
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
        ///     Gets the name of the mocked member.
        /// </summary>
        /// <value>The name of the mocked member.</value>
        string MemberName { get; }

        /// <summary>
        ///     Gets the name of the property or method used to provide the mocked member with behaviour.
        /// </summary>
        /// <value>The name of the property or method used to provide the mocked member with behaviour.</value>
        string MemberMockName { get; }
    }
}
