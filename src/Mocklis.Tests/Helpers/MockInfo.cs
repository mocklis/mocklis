// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockInfo.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Helpers
{
    #region Using Directives

    using Mocklis.Core;

    #endregion

    public class MockInfo : IMockInfo
    {
        public static MockInfo Default { get; } =
            new MockInfo(null, "ClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient);

        public object MockInstance { get; }
        public string MocklisClassName { get; }
        public string InterfaceName { get; }
        public string MemberName { get; }
        public string MemberMockName { get; }
        public Strictness Strictness { get; }

        public MockInfo(object mockInstance, string mocklisClassName, string interfaceName, string memberName, string memberMockName,
            Strictness strictness)
        {
            MockInstance = mockInstance;
            MocklisClassName = mocklisClassName;
            InterfaceName = interfaceName;
            MemberName = memberName;
            MemberMockName = memberMockName;
            Strictness = strictness;
        }
    }
}
