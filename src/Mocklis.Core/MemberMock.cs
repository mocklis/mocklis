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

    public abstract class MemberMock
    {
        public object MockInstance { get; }
        public string MocklisClassName { get; }
        public string InterfaceName { get; }
        public string MemberName { get; }
        public string MemberMockName { get; }

        protected internal MemberMock(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName)
        {
            MockInstance = mockInstance;
            MocklisClassName = mocklisClassName;
            InterfaceName = interfaceName ?? throw new ArgumentNullException(nameof(interfaceName));
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
            MemberMockName = memberMockName ?? throw new ArgumentNullException(nameof(memberMockName));
        }
    }
}
