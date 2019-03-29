// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentValuePropertyCheck_Verify_should.cs">
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
    using Mocklis.Verification.Checks;
    using Xunit;

    #endregion

    public class CurrentValuePropertyCheck_Verify_should
    {
        [Fact]
        private void use_current_culture_in_successful_description()
        {
            // Arrange
            var store = new MockStoredProperty<DateTime>();
            store.Value.Stored(20190308.AtUtc(120931));

            var sut = new CurrentValuePropertyCheck<DateTime>(store, "TestName", 20190308.AtUtc(120931));

            // Act
            using (Scope.CurrentCulture(CultureInfo.InvariantCulture))
            {
                var result = sut.Verify().FirstOrDefault();

                // Assert;
                Assert.True(result.Success);
                Assert.Equal("Value check 'TestName': Expected '03/08/2019 12:09:31'; Current Value is '03/08/2019 12:09:31'", result.Description);
            }
        }

        [Fact]
        private void use_given_culture_in_successful_description()
        {
            // Arrange
            var store = new MockStoredProperty<DateTime>();
            store.Value.Stored(20190308.AtUtc(120931));

            var sut = new CurrentValuePropertyCheck<DateTime>(store, "TestName", 20190308.AtUtc(120931));

            // Act
            var result = sut.Verify(CultureInfo.GetCultureInfo("en-GB")).FirstOrDefault();

            // Assert;
            Assert.True(result.Success);
            Assert.Equal("Value check 'TestName': Expected '08/03/2019 12:09:31'; Current Value is '08/03/2019 12:09:31'", result.Description);
        }

        [Fact]
        private void use_given_culture_in_failed_description()
        {
            // Arrange
            var store = new MockStoredProperty<DateTime>();
            store.Value.Stored(20190308.AtUtc(120931));

            var sut = new CurrentValuePropertyCheck<DateTime>(store, "TestName", 20190308.AtUtc(145512));

            // Act
            var result = sut.Verify(CultureInfo.GetCultureInfo("de-DE")).FirstOrDefault();

            // Assert;
            Assert.False(result.Success);
            Assert.Equal("Value check 'TestName': Expected '08.03.2019 14:55:12'; Current Value is '08.03.2019 12:09:31'", result.Description);
        }
    }
}
