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
    class GroupRequestTests : TestBase
    {
        private const string _testGroupName = "___testGroupName";
        private List<long> _newGroups = new List<long>();

        private ObjectResponse<NamedObject> DoAddGroup(string name, bool globalGroup)
        {
            var req = GroupRequestFactory.CreateAddGroupRequest(name, globalGroup);
            var response = RequestHandler.Execute<ObjectResponse<NamedObject>>(req);
            if (response != null && response.Success)
            {
                _newGroups.Add(response.Data.Id); //for cleanup
            }
            return response;
        }

        [TearDown]
        public void RemoveAddedGroups()
        {
            //TODO: get the group with _testGroupName and remove it when found instead of using _newGroups
            foreach (long id in _newGroups)
            {
                var req = GroupRequestFactory.CreateDeleteGroupRequest(id);
                RequestHandler.SuccessfullyExecute<DataspaceResponse>(req);
            }
            _newGroups.Clear();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void AddGroup(bool globalGroup)
        {
            var response = DoAddGroup(_testGroupName, globalGroup);
            Assert.IsTrue(response.Success, response.Message);
            Assert.AreEqual(_testGroupName, response.Data.Name, "Name differs");
            Assert.Greater(response.Data.Id, 0, "Id is invalid");
        }

        [TestCase(true, false, true)]
        [TestCase(false, true, true)]
        [TestCase(true, true, false)]
        [TestCase(false, false, false)]
        public void AddGroupTwice(bool firstGlobal, bool secondGlobal, bool shouldWork)
        {
            var firstResponse = DoAddGroup(_testGroupName, firstGlobal);
            //first request should always work
            Assert.IsTrue(firstResponse.Success);
            var secondResponse = DoAddGroup(_testGroupName, secondGlobal);
            Assert.AreEqual(shouldWork,  secondResponse.Success, "Adding group twice works different than intended!");
            if (secondResponse.Success)
            {
                Assert.AreEqual(firstResponse.Data.Name, secondResponse.Data.Name);
                Assert.AreNotEqual(firstResponse.Data.Id, secondResponse.Data.Id);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GroupsAreAddedCorrectly(bool globalGroup)
        {
            // add a group first to make sure we have one. adding is tested in another test
            var addGroupResponse = DoAddGroup(_testGroupName, globalGroup);
            var req = GroupRequestFactory.CreateGetGroupsRequest(globalGroup);
            var response = RequestHandler.SuccessfullyExecute<ObjectResponse<List<NamedObject>>>(req);
            Assert.IsNotNull(response.Data);
            Assert.True(response.Data.Any());
            Assert.True(response.Data.Contains(addGroupResponse.Data), "GetGroups didn't get the test group");
        }
    }
}
