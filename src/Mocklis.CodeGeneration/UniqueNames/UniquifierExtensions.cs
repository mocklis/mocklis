// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UniquifierExtensions.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.UniqueNames
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public static class UniquifierExtensions
    {
        public static IEnumerable<(string uniqueName, T item)> GetUniqueNames<T>(this Uniquifier uniquifier, IEnumerable<T> items)
            where T : IHasPreferredName
        {
            T[] array = items.ToArray();
            foreach (var item in array)
            {
                uniquifier.ReserveName(item.PreferredName);
            }

            foreach (var item in array)
            {
                yield return (uniquifier.GetUniqueName(item.PreferredName), item);
            }
        }
    }
}
