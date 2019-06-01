// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuePropertyCheck_should.cs">
//   SPDX-License-Identifier: MIT
//   Copyright © 2019 Esbjörn Redmo and contributors. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Mocklis.Tests.Verification.Checks
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Linq;
    using Mocklis.Tests.Helpers;
    using Mocklis.Tests.Mocks;
    using Mocklis.Verification;
    using Mocklis.Verification.Checks;
    using Xunit;

    #endregion

    public class CurrentValuePropertyCheck_should
    {
        private MockMembers MockMembers { get; }
        private VerificationGroup Group { get; }

        public CurrentValuePropertyCheck_should()
        {
            MockMembers = new MockMembers();
            Group = new VerificationGroup();
        }

        [Fact]
        public void RequireStoredProperty()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => new CurrentValuePropertyCheck<string>(null, null, null));
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
            var groupResult = ((IVerifiable)Group).Verify(CultureInfo.GetCultureInfo("en-GB")).FirstOrDefault();

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
            var groupResult = ((IVerifiable)Group).Verify(CultureInfo.GetCultureInfo("de-DE")).FirstOrDefault();

            // Assert
            var result = Assert.Single(groupResult.SubResults);
            Assert.False(result.Success);
            Assert.Equal("Value check 'TestName': Expected '08.03.2019 14:55:12'; Current Value is '08.03.2019 12:09:31'", result.Description);
        }

        [Fact]
        public void UseGivenEqualityComparer()
        {
            // Arrange
            MockMembers.StringProperty.Stored("tomeyto").CurrentValueCheck(Group, null, "tomahto", new StringLengthComparer());

            // Act
            var groupResult = ((IVerifiable)Group).Verify().FirstOrDefault();

            // Assert
            var result = Assert.Single(groupResult.SubResults);
            Assert.True(result.Success);
        }
    }
}
