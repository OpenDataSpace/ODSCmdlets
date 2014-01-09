﻿using NUnit.Framework;
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
            var requestHandler = new RequestHandler(login.UserName, login.Password, login.URL);
            Assert.False(String.IsNullOrEmpty(requestHandler.Login()));
        }

    }
}
