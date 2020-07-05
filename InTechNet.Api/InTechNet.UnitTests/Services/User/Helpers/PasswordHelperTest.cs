using FluentAssertions;
using InTechNet.Services.User.Helpers;
using Xbehave;

namespace InTechNet.UnitTests.Services.User.Helpers
{
    /// <summary>
    /// PasswordHelperTest testing methods
    /// </summary>
    public class PasswordHelperTest
    {
        /// <summary>
        /// Assert the behavior of the helper method when determining whether a password
        /// is hard or not
        /// </summary>
        [Scenario]
        public void IsStrongEnoughTrueOnHardPassword(string password)
        {
            "Given a password that as 8 >= x >= 64 chars, contains an upper- and lowercase and a digit"
                .x(()
                    => password = "12345678910NiceTry");

            "Then the helper method should acknowledge the strength of it"
                .x(()
                    => PasswordHelper.IsStrongEnough(password)
                        .Should()
                        .BeTrue());
        }

        /// <summary>
        /// Assert the behavior of the helper method when determining whether a password
        /// is hard or not
        /// </summary>
        [Scenario]
        // Too short password
        [Example("123Aa")]
        // Too long password (64 ones and letters)
        [Example("11111111111111111111111111111111111111111111111111111111111111111Aa")]
        // Missing uppercase
        [Example("123456789a")]
        // Missing lowercase
        [Example("123456789A")]
        public void IsStrongEnoughFalseOnWeakPassword(string password)
        {
            "Given a password that does not respect all constraints for secured passwords Then the helper method should not acknowledge it strength"
                .x(()
                    => PasswordHelper.IsStrongEnough(password)
                        .Should()
                        .BeFalse());
        }
    }
}
