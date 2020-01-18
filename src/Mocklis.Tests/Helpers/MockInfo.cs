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

    public sealed class MockInfo : IMockInfo
    {
        public static MockInfo Lenient { get; } =
            new MockInfo(new object(), "ClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Lenient);

        public static MockInfo Strict { get; } =
            new MockInfo(new object(), "ClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.Strict);

        public static MockInfo VeryStrict { get; } =
            new MockInfo(new object(), "ClassName", "InterfaceName", "MemberName", "MemberMockName", Strictness.VeryStrict);

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
