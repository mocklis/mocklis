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
        public string InterfaceName { get; }
        public string MemberName { get; }
        public string MemberMockName { get; }

        protected internal MemberMock(string interfaceName, string memberName, string memberMockName)
        {
            InterfaceName = interfaceName ?? throw new ArgumentNullException(nameof(interfaceName));
            MemberName = memberName ?? throw new ArgumentNullException(nameof(memberName));
            MemberMockName = memberMockName ?? throw new ArgumentNullException(nameof(memberMockName));
        }
    }
}
