// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockSettings.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator.CodeGeneration;

public record struct MockSettings(bool MockReturnsByRef, bool MockReturnsByRefReadonly, bool Strict, bool VeryStrict);
