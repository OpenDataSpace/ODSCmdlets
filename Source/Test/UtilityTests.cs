using NUnit.Framework;
using OpenDataSpace.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;

namespace Test
{
    [TestFixture]
    class UtilityTests
    {

        [TestCase("foobar")]
        [TestCase("")]
        public void SecureStringConversion(string input)
        {
            SecureString secureInput = Utility.StringToSecureString(input);
            Assert.AreEqual(input, Utility.SecureStringToString(secureInput));
        }
    }
}
