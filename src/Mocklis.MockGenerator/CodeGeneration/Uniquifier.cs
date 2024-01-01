// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Uniquifier.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

public sealed class Uniquifier
{
    private readonly HashSet<string> _reservedNames = new();
    private readonly HashSet<string> _usedNames = new();

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

        for (var i = 0;; i++)
        {
            var candidateName = FormattableString.Invariant($"{name}{i}");
            if (_reservedNames.Contains(candidateName) || _usedNames.Contains(candidateName))
            {
                continue;
            }

            _usedNames.Add(candidateName);

            return candidateName;
        }
    }
}
