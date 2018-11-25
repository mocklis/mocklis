// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberMock.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    public abstract class MemberMock : IMockInfo
    {
        private readonly object _mockInstance;
        private readonly string _mocklisClassName;
        private readonly string _interfaceName;
        private readonly string _memberName;
        private readonly string _memberMockName;

        object IMockInfo.MockInstance => _mockInstance;
        string IMockInfo.MocklisClassName => _mocklisClassName;
        string IMockInfo.InterfaceName => _interfaceName;
        string IMockInfo.MemberName => _memberName;
        string IMockInfo.MemberMockName => _memberMockName;

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
