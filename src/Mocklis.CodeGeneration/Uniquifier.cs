// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Uniquifier.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using static System.FormattableString;

    #endregion

    public interface IHasPreferredName
    {
        string PreferredName { get; }
    }

    public sealed class Uniquifier
    {
        public static IEnumerable<(string uniqueName, T item)> GetUniqueNames<T>(IEnumerable<T> items) where T : IHasPreferredName
        {
            T[] array = items.ToArray();
            var uniquifier = new Uniquifier();
            foreach (var item in array)
            {
                uniquifier.ReserveName(item.PreferredName);
            }

            foreach (var item in array)
            {
                yield return (uniquifier.GetUniqueName(item.PreferredName), item);
            }
        }

        private readonly HashSet<string> _reservedNames = new HashSet<string>();
        private readonly HashSet<string> _usedNames = new HashSet<string>();

        public Uniquifier()
        {
        }

        public Uniquifier(IEnumerable<string> usedNames)
        {
            var usedNamesArray = usedNames.ToArray();
            foreach (var usedName in usedNamesArray)
            {
                ReserveName(usedName);
            }

            foreach (var usedName in usedNamesArray)
            {
                GetUniqueName(usedName);
            }
        }

        public void ReserveName(string name)
        {
            _reservedNames.Add(name);
        }

        public string GetUniqueName(string name)
        {
            if (!_usedNames.Contains(name))
            {
                _usedNames.Add(name);
                return name;
            }

            for (int i = 0;; i++)
            {
                string candidateName = Invariant($"{name}_{i}");
                if (_reservedNames.Contains(candidateName) || _usedNames.Contains(candidateName))
                {
                    continue;
                }

                _usedNames.Add(candidateName);

                return candidateName;
            }
        }
    }
}
