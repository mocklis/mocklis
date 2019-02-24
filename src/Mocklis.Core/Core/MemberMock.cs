// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberMock.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    ///     Abstract base class for all Mocklis mock classes.
    ///     Implements the <see cref="Mocklis.Core.IMockInfo" /> interface.
    /// </summary>
    /// <seealso cref="Mocklis.Core.IMockInfo" />
    public abstract class MemberMock : IMockInfo
    {
        private readonly object _mockInstance;
        private readonly string _mocklisClassName;
        private readonly string _interfaceName;
        private readonly string _memberName;
        private readonly string _memberMockName;

        /// <summary>
        ///     Gets the instance of the mocklis class through with the mocked member is accessed.
        /// </summary>
        /// <value>The instance of the mocklis class through with the mocked member is accessed.</value>
        object IMockInfo.MockInstance => _mockInstance;

        /// <summary>
        ///     Gets the name of the mocklis class.
        /// </summary>
        /// <value>The name of the mocklis class.</value>
        string IMockInfo.MocklisClassName => _mocklisClassName;

        /// <summary>
        ///     Gets the name of the interface on which the mocked member is defined.
        /// </summary>
        /// <value>The name of the interface on which the mocked member is defined.</value>
        string IMockInfo.InterfaceName => _interfaceName;

        /// <summary>
        ///     Gets the name of the mocked member.
        /// </summary>
        /// <value>The name of the mocked member.</value>
        string IMockInfo.MemberName => _memberName;

        /// <summary>
        ///     Gets the name of the property or method used to provide the mocked member with behaviour.
        /// </summary>
        /// <value>The name of the property or method used to provide the mocked member with behaviour.</value>
        string IMockInfo.MemberMockName => _memberMockName;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemberMock" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mocked member is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked member is defined.</param>
        /// <param name="memberName">The name of the mocked member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mocked member with behaviour.</param>
        protected internal MemberMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
        {
            _mockInstance = mockInstance ?? throw new ArgumentNullException(nameof(mockInstance));
            _mocklisClassName = mocklisClassName ?? throw new ArgumentNullException(nameof(mocklisClassName));
            _interfaceName = interfaceName ?? throw new ArgumentNullException(nameof(interfaceName));
            _memberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
            _memberMockName = memberMockName ?? throw new ArgumentNullException(nameof(memberMockName));
        }
    }
}
