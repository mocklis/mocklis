// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockType.cs">
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Core
{
    public enum MockType
    {
        Method = 0,
        PropertyGet = 2,
        PropertySet = 3,
        EventAdd = 4,
        EventRemove = 5,
        IndexerGet = 6,
        IndexerSet = 7,
        VirtualMethod = 8,
        VirtualPropertyGet = 10,
        VirtualIndexerGet = 14
    }
}
