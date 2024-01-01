// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITypeParameterSubstitutions.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2024 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

public interface ITypeParameterSubstitutions
{
    string FindSubstitution(string typeParameterName);
}
