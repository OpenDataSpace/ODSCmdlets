using NUnit.Framework;
using OpenDataSpace.Commands.Objects;
using OpenDataSpace.Commands.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    [TestFixture]
    class UserRequestTests : TestBase
    {
        // TODO: better cleanup for added users!

        private UserObject GetAbritraryUser()
        {
            var listRequest = UserRequestFactory.CreateQueryUserRequest(0, 1, "", "", "", "");
            var list = RequestHandler.ExecuteAndUnpack<List<UserObject>>(listRequest);
            return list[0];
        }

        private void RemoveUser(long id)
        {
            var request = UserRequestFactory.CreateDeleteUserRequest(id);
            RequestHandler.ExecuteSuccessfully<DataspaceResponse>(request);
        }

        [Test]
        public void QueryAllUsers()
        {
            var request = UserRequestFactory.CreateQueryUserRequest(0, 10, "", "", "", "");
            var list = RequestHandler.ExecuteAndUnpack<List<UserObject>>(request);
            // we have user, at least the one that is used for log in is always there
            Assert.Greater(list.Count, 0);
            var user = list[0] as UserObject;
            // make sure we have data that makes sense
            // TODO: once we know what the user really provides, check that email, name, etc arent null
            Assert.NotNull(user);
            Assert.IsNotNullOrEmpty(user.UserName);
        }

        // TODO: After adding two users, we can test start and limit parameters

        // TODO: After adding two users, we can test queries

        [Test]
        public void GetUserbyId()
        {

            var user = GetAbritraryUser();
            var request = UserRequestFactory.CreateGetUserRequest(user.Id);
            var getUser = RequestHandler.ExecuteAndUnpack<UserObject>(request);
            Assert.True(user.Equals(getUser));
            Assert.True(user.EqualsCompletely(getUser));
        }


        [Test, Explicit("Pollutes DataSpace, because deleting is just deactivation")]
        public void AddUser()
        {
            var addable = GetRandomUserData();
            var request = UserRequestFactory.CreateAddUserRequest(addable);
            var added = RequestHandler.ExecuteAndUnpack<UserObject>(request);
            Assert.Greater(added.Id, 0, "User has no proper ID");
            Assert.True(addable.EqualsCompletely(added), "Added users has incorrect information");
            RemoveUser(added.Id);
        }

        [Test, Explicit("Pollutes DataSpace, because deleting is just deactivation")]
        public void EditUser()
        {
            var addable = GetRandomUserData();
            var addRequest = UserRequestFactory.CreateAddUserRequest(addable);
            var added = RequestHandler.ExecuteAndUnpack<UserObject>(addRequest);

            var updates = GetRandomUserData();
            var request = UserRequestFactory.CreateEditUserRequest(added.Id, updates);
            var updated = RequestHandler.ExecuteAndUnpack<UserObject>(request);
            Assert.AreEqual(added, updated);
            Assert.False(updated.EqualsCompletely(added));
            // notice that the UserName never changes, this is intended
            updates.UserName = added.UserName;
            Assert.True(updates.EqualsCompletely(updated));
            RemoveUser(updated.Id);
        }

        [Test, Explicit("Pollutes DataSpace, because deleting is just deactivation")]
        public void RemoveUser()
        {
            var addable = GetRandomUserData();
            var addRequest = UserRequestFactory.CreateAddUserRequest(addable);
            var added = RequestHandler.ExecuteAndUnpack<UserObject>(addRequest);
            var request = UserRequestFactory.CreateDeleteUserRequest(added.Id);
            var response = RequestHandler.Execute<DataspaceResponse>(request);
            Assert.True(response.Success, response.Message);
        
            var getRequest = UserRequestFactory.CreateGetUserRequest(added.Id);
            var result = RequestHandler.ExecuteAndUnpack<UserObject>(getRequest);
            added.Locked = true;
            added.EqualsCompletely(result);
        }
    }
}
