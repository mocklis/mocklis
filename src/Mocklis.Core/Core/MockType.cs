// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockType.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    /// <summary>
    ///     Enumeration for the 'type' of a mock usage.
    /// </summary>
    public enum MockType
    {
        /// <summary>
        ///     Value indicating calling a method implemented as a mock property.
        /// </summary>
        Method = 0,

        /// <summary>
        ///     Value indicating getting a value from a property implemented as a mock property.
        /// </summary>
        PropertyGet = 2,

        /// <summary>
        ///     Value indicating setting a value on a property implemented as a mock property.
        /// </summary>
        PropertySet = 3,

        /// <summary>
        ///     Value indicating adding an event handler to an event implemented as a mock property.
        /// </summary>
        EventAdd = 4,

        /// <summary>
        ///     Value indicating removing an event handler from an event implemented as a mock property.
        /// </summary>
        EventRemove = 5,

        /// <summary>
        ///     Value indicating setting a value on an indexer implemented as a mock property.
        /// </summary>
        IndexerGet = 6,

        /// <summary>
        ///     Value indicating getting a value from an indexer implemented as a mock property.
        /// </summary>
        IndexerSet = 7,

        /// <summary>
        ///     Value indicating calling a method implemented as a virtual method.
        /// </summary>
        VirtualMethod = 8,

        /// <summary>
        ///     Value indicating getting a value from a property implemented as virtual methods.
        /// </summary>
        VirtualPropertyGet = 10,

        /// <summary>
        ///     Value indicating setting a value on a property implemented as virtual methods.
        /// </summary>
        VirtualPropertySet = 11,

        /// <summary>
        ///     Value indicating getting a value from an indexer implemented as virtual methods.
        /// </summary>
        VirtualIndexerGet = 14,

        /// <summary>
        ///     Value indicating setting a value on an indexer implemented as virtual methods.
        /// </summary>
        VirtualIndexerSet = 15
    }
}
