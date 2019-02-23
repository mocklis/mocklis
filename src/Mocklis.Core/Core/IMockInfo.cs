// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMockInfo.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Interface that models the mock through which a mocked member is accessed.
    /// </summary>
    public interface IMockInfo
    {
        /// <summary>
        ///     Gets the instance of the mocklis class.
        /// </summary>
        /// <value>The instance of the mocklis class.</value>
        object MockInstance { get; }

        /// <summary>
        ///     Gets the name of the mocklis class.
        /// </summary>
        /// <value>The name of the mocklis class.</value>
        string MocklisClassName { get; }

        /// <summary>
        ///     Gets the name of the interface on which the accessed member is defined.
        /// </summary>
        /// <value>The name of the interface on which the accessed member is defined.</value>
        string InterfaceName { get; }

        /// <summary>
        ///     Gets the name of the accessed member.
        /// </summary>
        /// <value>The name of the accessed member.</value>
        string MemberName { get; }

        /// <summary>
        ///     Gets the name of the property or method used to provide the mock with behaviour for this member.
        /// </summary>
        /// <value>The name of the property or method used to provide the mock with behaviour for this member.</value>
        string MemberMockName { get; }
    }
}
