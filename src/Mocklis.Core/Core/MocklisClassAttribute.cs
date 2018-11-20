// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassAttribute.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MocklisClassAttribute : Attribute
    {
    }
}
