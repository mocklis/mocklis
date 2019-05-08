// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberMock.cs">
//   SPDX-License-Identifier: MIT
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
    ///     Implements the <see cref="IMockInfo" /> interface.
    /// </summary>
    /// <seealso cref="IMockInfo" />
    public abstract class MemberMock : IMockInfo
    {
        private readonly object _mockInstance;
        private readonly string _mocklisClassName;
        private readonly string _interfaceName;
        private readonly string _memberName;
        private readonly string _memberMockName;
        private readonly Strictness _strictness;

        /// <summary>
        ///     Gets the instance of the mocklis class through with the mock is accessed.
        /// </summary>
        /// <value>The instance of the mocklis class through with the mock is accessed.</value>
        object IMockInfo.MockInstance => _mockInstance;

        /// <summary>
        ///     Gets the name of the mocklis class.
        /// </summary>
        /// <value>The name of the mocklis class.</value>
        string IMockInfo.MocklisClassName => _mocklisClassName;

        /// <summary>
        ///     Gets the name of the interface on which the mocked interface member is defined.
        /// </summary>
        /// <value>The name of the interface on which the mocked interface member is defined.</value>
        string IMockInfo.InterfaceName => _interfaceName;

        /// <summary>
        ///     Gets the name of the mocked interface member.
        /// </summary>
        /// <value>The name of the mocked interface member.</value>
        string IMockInfo.MemberName => _memberName;

        /// <summary>
        ///     Gets the name of the property or method used to provide the mock with behaviour.
        /// </summary>
        /// <value>The name of the property or method used to provide the mock with behaviour.</value>
        string IMockInfo.MemberMockName => _memberMockName;

        /// <summary>
        ///     Gets the strictness of the mock.
        /// </summary>
        /// <value>The strictness of the mock.</value>
        Strictness IMockInfo.Strictness => _strictness;

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemberMock" /> class.
        /// </summary>
        /// <param name="mockInstance">The instance of the mocklis class through with the mock is accessed.</param>
        /// <param name="mocklisClassName">The name of the mocklis class.</param>
        /// <param name="interfaceName">The name of the interface on which the mocked interface  member is defined.</param>
        /// <param name="memberName">The name of the mocked interface member.</param>
        /// <param name="memberMockName">The name of the property or method used to provide the mock with behaviour.</param>
        /// <param name="strictness">The strictness of the mock.</param>
        protected internal MemberMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName,
            Strictness strictness)
        {
            _mockInstance = mockInstance ?? throw new ArgumentNullException(nameof(mockInstance));
            _mocklisClassName = mocklisClassName ?? throw new ArgumentNullException(nameof(mocklisClassName));
            _interfaceName = interfaceName ?? throw new ArgumentNullException(nameof(interfaceName));
            _memberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
            _memberMockName = memberMockName ?? throw new ArgumentNullException(nameof(memberMockName));
            _strictness = strictness;
        }
    }
}
