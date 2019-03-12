// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassAttribute.cs">
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
    ///     Attribute used to identify classes that Mocklis should autogenerate the code for. This class cannot be inherited.
    ///     Inherits from the <see cref="Attribute" /> class.
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class MocklisClassAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MocklisClassAttribute" /> class.
        /// </summary>
        public MocklisClassAttribute()
        {
            MockReturnsByRef = false;
            MockReturnsByRefReadonly = true;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether members that return values by reference should be mocked using a mock
        ///     property.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mock properties should be used as a mocking strategy for members that return values by reference;
        ///     <c>false</c> if virtual methods should be used as a mocking strategy.
        /// </value>
        public bool MockReturnsByRef { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether members that return values by readonly reference should be mocked using a
        ///     mock property.
        /// </summary>
        /// <value>
        ///     <c>true</c> if mock properties should be used as a mocking strategy for members that return values by readonly
        ///     reference;
        ///     <c>false</c> if virtual methods should be used as a mocking strategy.
        /// </value>
        public bool MockReturnsByRefReadonly { get; set; }
    }
}
