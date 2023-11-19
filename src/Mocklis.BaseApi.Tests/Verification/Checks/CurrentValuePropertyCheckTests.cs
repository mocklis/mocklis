// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuePropertyCheckTests.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019-2023 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Verification.Checks
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Linq;
    using Mocklis.Helpers;
    using Mocklis.Mocks;
    using Xunit;

    #endregion

    public class CurrentValuePropertyCheckTests
    {
        private MockMembers MockMembers { get; }
        private VerificationGroup Group { get; }

        public CurrentValuePropertyCheckTests()
        {
            MockMembers = new MockMembers();
            Group = new VerificationGroup();
        }

        [Fact]
        public void RequireStoredProperty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CurrentValuePropertyCheck<string>(null!, null, ""));
            Assert.Equal("property", ex.ParamName);
        }

        [Fact]
        public void UseCurrentCultureInSuccessfulDescription()
        {
            // Arrange
            MockMembers.DateTimeProperty.Stored(20190308.AtUtc(120931)).CurrentValueCheck(Group, "TestName", 20190308.AtUtc(120931));

            // Act
            using (Scope.CurrentCulture(CultureInfo.InvariantCulture))
            {
                var groupResult = ((IVerifiable)Group).Verify().FirstOrDefault();

                // Assert
                var result = Assert.Single(groupResult.SubResults);
                Assert.True(result.Success);
                Assert.Equal("Value check 'TestName': Expected '03/08/2019 12:09:31'; Current Value is '03/08/2019 12:09:31'", result.Description);
            }
        }

        [Fact]
        public void UseGivenCultureInSuccessfulDescription()
        {
            // Arrange
            MockMembers.DateTimeProperty.Stored(20190308.AtUtc(120931)).CurrentValueCheck(Group, "TestName", 20190308.AtUtc(120931));

            // Act
            var groupResult = ((IVerifiable)Group).Verify(new CultureInfo("en-GB")).FirstOrDefault();

            // Assert
            var result = Assert.Single(groupResult.SubResults);
            Assert.True(result.Success);
            Assert.Equal("Value check 'TestName': Expected '08/03/2019 12:09:31'; Current Value is '08/03/2019 12:09:31'", result.Description);
        }

        [Fact]
        public void UseGivenCultureInFailedDescription()
        {
            // Arrange
            MockMembers.DateTimeProperty.Stored(20190308.AtUtc(120931)).CurrentValueCheck(Group, "TestName", 20190308.AtUtc(145512));

            // Act
            var groupResult = ((IVerifiable)Group).Verify(new CultureInfo("de-DE")).FirstOrDefault();

            // Assert
            var result = Assert.Single(groupResult.SubResults);
            Assert.False(result.Success);
            Assert.Equal("Value check 'TestName': Expected '08.03.2019 14:55:12'; Current Value is '08.03.2019 12:09:31'", result.Description);
        }

        [Fact]
        public void UseGivenEqualityComparer()
        {
            // Arrange
            // ReSharper disable StringLiteralTypo
            MockMembers.StringProperty.Stored("tomeyto").CurrentValueCheck(Group, null, "tomahto", new StringLengthComparer());
            // ReSharper restore StringLiteralTypo

            // Act
            var groupResult = ((IVerifiable)Group).Verify().FirstOrDefault();

            // Assert
            var result = Assert.Single(groupResult.SubResults);
            Assert.True(result.Success);
        }
    }
}
