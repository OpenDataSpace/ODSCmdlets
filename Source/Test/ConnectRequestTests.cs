using NUnit.Framework;
using OpenDataSpace.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    [TestFixture]
    class ConnectRequestTests : TestBase
    {

        [Test]
        public void LoginTest()
        {
            LoginData login = DefaultLoginData;
            var requestHandler = new RequestHandler(login.UserName, login.Password, login.URL);
            Assert.IsNotNullOrEmpty(requestHandler.Login());
        }

        [Test]
        public void LogoutTest()
        {
            LoginData login = DefaultLoginData;
            var requestHandler = new RequestHandler(login.UserName, login.Password, login.URL);
            requestHandler.Login();
            Assert.True(requestHandler.Logout());
        }

        [Test]
        public void LogoutWithoutLoginTest()
        {
            LoginData login = DefaultLoginData;
            var requestHandler = new RequestHandler(login.UserName, login.Password, login.URL);
            Assert.False(requestHandler.Logout());
        }

    }
}
