// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassAttribute.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    #region Using Directives

    using System;

    #endregion

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class MocklisClassAttribute : Attribute
    {
        public MocklisClassAttribute()
        {
            MockReturnsByRef = false;
            MockReturnsByRefReadonly = true;
        }

        public bool MockReturnsByRef { get; set; }

        public bool MockReturnsByRefReadonly { get; set; }
    }
}
