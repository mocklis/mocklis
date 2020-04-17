// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassUpdateTestCase.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2020 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.MockGenerator
{
    using System;
    using System.IO;
    using Xunit.Abstractions;

    [Serializable]
    public class ClassUpdateTestCase : IXunitSerializable
    {
        public string PathToTestCases { get; set; } = string.Empty;
        public string TestCaseFolder { get; set; } = string.Empty;
        public string TestCase { get; set; } = string.Empty;

        public override string ToString()
        {
            return TestCaseFolder + "\\" + TestCase;
        }

        public void Deserialize(IXunitSerializationInfo info)
        {
            PathToTestCases = info.GetValue<string>(nameof(PathToTestCases));
            TestCaseFolder = info.GetValue<string>(nameof(TestCaseFolder));
            TestCase = info.GetValue<string>(nameof(TestCase));
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue(nameof(PathToTestCases), PathToTestCases);
            info.AddValue(nameof(TestCaseFolder), TestCaseFolder);
            info.AddValue(nameof(TestCase), TestCase);
        }

        public string SourceFileName => Path.Combine(PathToTestCases, TestCaseFolder, TestCase + ".cs");
        public string ExpectedFileName => Path.Combine(PathToTestCases, TestCaseFolder, TestCase + ".Expected.cs");
    }
}