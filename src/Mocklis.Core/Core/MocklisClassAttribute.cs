// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MocklisClassAttribute.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
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
            Strict = false;
            VeryStrict = false;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether members that don't have any steps defined should silently return
        ///     a default value (or do nothing) or throw a <see cref="MockMissingException" /> when accessed.
        /// </summary>
        /// <value>
        ///     <c>true</c> if a <see cref="MockMissingException" /> should be thrown.
        ///     <c>false</c> if a default value should be returned (if there is a value to be returned). This is the default.
        /// </value>
        public bool Strict { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether defined step that don't have any subsequent steps defined should
        ///     silently return a default value (or do nothing) or throw a <see cref="MockMissingException" /> when accessed.
        ///     Note <see cref="Strict" /> is implicitly treated as <c>true</c> if <see cref="VeryStrict" /> is <c>true</c>.
        /// </summary>
        /// <remarks>
        ///     The difference between <see cref="Strict" /> and <see cref="VeryStrict" /> is how a partially configured mock
        ///     is treated. If you just add a Log step, and only <see cref="Strict" /> is <c>true</c> then no exception would
        ///     be thrown even though Log cannot return a value on its own and requires subsequent steps to provide behaviour.
        ///     If <see cref="VeryStrict" /> is <c>true</c> then this case would throw an exception.
        /// </remarks>
        /// <value>
        ///     <c>true</c> if a <see cref="MockMissingException" /> should be thrown.
        ///     <c>false</c> if a default value should be returned (if there is a value to be returned). This is the default.
        /// </value>
        public bool VeryStrict { get; set; }

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
