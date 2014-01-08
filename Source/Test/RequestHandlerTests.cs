using NUnit.Framework;
using OpenDataSpace.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    [TestFixture]
    class RequestHandlerTests : TestBase
    {

        [Test]
        public void LoginTest()
        {
            LoginData login = DefaultLoginData;
            var requestHandler = new RequestHandler(login.Username, login.Password, login.Host);
            Assert.False(String.IsNullOrEmpty(requestHandler.Login()));
        }

    }
}
