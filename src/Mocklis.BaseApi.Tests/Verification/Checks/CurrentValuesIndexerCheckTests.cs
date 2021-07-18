// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuesIndexerCheckTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2021 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Checks
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Mocklis.Helpers;
    using Mocklis.Interfaces;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class CurrentValuesIndexerCheckTests
    {
        private MockMembers MockMembers { get; }
        private IIndexers Indexers { get; }
        private VerificationGroup Group { get; }

        public CurrentValuesIndexerCheckTests()
        {
            Indexers = MockMembers = new MockMembers();
            Group = new VerificationGroup();
        }

        [Fact]
        public void RequireStoredProperty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CurrentValuesIndexerCheck<string, string>(null!, null, null));
            Assert.Equal("indexer", ex.ParamName);
        }

        [Fact]
        public void UseCurrentCultureInSuccessfulDescription()
        {
            // Arrange
            MockMembers.Item0
                .StoredAsDictionary()
                .CurrentValuesCheck(Group, "TestName", new[] { new KeyValuePair<bool, DateTime>(false, 20190308.AtUtc(120931)) });

            Indexers[false] = 20190308.AtUtc(120931);

            // Act
            using (Scope.CurrentCulture(CultureInfo.InvariantCulture))
            {
                var groupResult = ((IVerifiable)Group).Verify();

                // Assert
                var result = Assert.Single(groupResult);
                result.AssertEquals(new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Values check 'TestName':", new[]
                    {
                        new VerificationResult("Key 'False'; Expected '03/08/2019 12:09:31'; Current Value is '03/08/2019 12:09:31'", true)
                    })
                }));
            }
        }

        [Fact]
        public void UseGivenCultureInSuccessfulDescription()
        {
            // Arrange
            MockMembers.Item0
                .StoredAsDictionary()
                .CurrentValuesCheck(Group, "TestName", new[] { new KeyValuePair<bool, DateTime>(false, 20190308.AtUtc(120931)) });

            Indexers[false] = 20190308.AtUtc(120931);

            // Act
            var groupResult = ((IVerifiable)Group).Verify(new CultureInfo("en-GB"));

            // Assert
            var result = Assert.Single(groupResult);
            result.AssertEquals(new VerificationResult("Verification Group:", new[]
            {
                new VerificationResult("Values check 'TestName':", new[]
                {
                    new VerificationResult("Key 'False'; Expected '08/03/2019 12:09:31'; Current Value is '08/03/2019 12:09:31'", true)
                })
            }));
        }

        [Fact]
        public void UseGivenCultureInFailedDescription()
        {
            // Arrange
            MockMembers.Item0
                .StoredAsDictionary()
                .CurrentValuesCheck(Group, "TestName", new[] { new KeyValuePair<bool, DateTime>(false, 20190308.AtUtc(145512)) });

            Indexers[false] = 20190308.AtUtc(120931);

            // Act
            var groupResult = ((IVerifiable)Group).Verify(new CultureInfo("de-DE"));

            // Assert
            var result = Assert.Single(groupResult);
            result.AssertEquals(new VerificationResult("Verification Group:", new[]
            {
                new VerificationResult("Values check 'TestName':", new[]
                {
                    new VerificationResult("Key 'False'; Expected '08.03.2019 14:55:12'; Current Value is '08.03.2019 12:09:31'", false)
                })
            }));
        }

        [Fact]
        public void UseGivenEqualityComparer()
        {
            // Arrange
            MockMembers.Item.StoredAsDictionary()
                .CurrentValuesCheck(Group, null, new[] { new KeyValuePair<int, string>(0, "tomahto") }, new StringLengthComparer());
            Indexers[0] = "tomeyto";

            // Act
            var groupResult = ((IVerifiable)Group).Verify().FirstOrDefault();

            // Assert
            var result = Assert.Single(groupResult.SubResults);
            Assert.True(result.Success);
        }

        [Fact]
        public void CheckMultipleValues()
        {
            // Arrange
            MockMembers.Item
                .StoredAsDictionary()
                .CurrentValuesCheck(Group, "Indexer Values", new[]
                {
                    new KeyValuePair<int, string>(1, "The first one"),
                    new KeyValuePair<int, string>(2, "The second one"),
                    new KeyValuePair<int, string>(3, "The third one")
                });

            Indexers[1] = "The first one";
            Indexers[2] = "The middle one";
            Indexers[3] = "The last one";

            // Act
            var ex = Assert.Throws<VerificationFailedException>(() => Group.Assert());

            // Assert
            ex.VerificationResult.AssertEquals(
                new VerificationResult("Verification Group:", new[]
                {
                    new VerificationResult("Values check 'Indexer Values':", new[]
                    {
                        new VerificationResult("Key '1'; Expected 'The first one'; Current Value is 'The first one'", true),
                        new VerificationResult("Key '2'; Expected 'The second one'; Current Value is 'The middle one'", false),
                        new VerificationResult("Key '3'; Expected 'The third one'; Current Value is 'The last one'", false)
                    })
                })
            );
        }

        [Fact]
        public void EnumerateExpectedValuesWhenVerifying()
        {
            // Arrange
            var expected = new List<KeyValuePair<int, string>>();

            MockMembers.Item
                .StoredAsDictionary()
                .CurrentValuesCheck(Group, "Indexer Values", expected);

            Indexers[1] = "Aha";
            expected.Add(new KeyValuePair<int, string>(1, "Aha"));

            // Act
            var groupResult = ((IVerifiable)Group).Verify();

            // Assert
            var result = Assert.Single(groupResult);
            result.AssertEquals(new VerificationResult("Verification Group:", new[]
            {
                new VerificationResult("Values check 'Indexer Values':", new[]
                {
                    new VerificationResult("Key '1'; Expected 'Aha'; Current Value is 'Aha'", true)
                })
            }));
        }
    }
}
