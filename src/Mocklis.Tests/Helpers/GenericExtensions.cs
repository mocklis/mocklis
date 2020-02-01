// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericExtensions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#if !NETCOREAPP1_1
namespace Mocklis.Tests.Helpers
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

    #endregion

    public static class GenericExtensions
    {
        public static T RoundTripWithBinaryFormatter<T>(this T item)
        {
            var formatter = new BinaryFormatter();

            using (var m = new MemoryStream())
            {
                formatter.Serialize(m, item);
                m.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(m);
            }
        }
    }
}

#endif
