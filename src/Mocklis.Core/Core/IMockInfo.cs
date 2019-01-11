// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IMockInfo.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public interface IMockInfo
    {
        object MockInstance { get; }
        string MocklisClassName { get; }
        string InterfaceName { get; }
        string MemberName { get; }
        string MemberMockName { get; }
    }
}
