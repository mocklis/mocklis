// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Uniquifier.cs">
//   Copyright © 2018 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.CodeGeneration.UniqueNames
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion

    public sealed class Uniquifier
    {
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
                string candidateName = FormattableString.Invariant($"{name}_{i}");
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
