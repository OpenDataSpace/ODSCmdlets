To be able to run the tests, add a file "test_login.txt" in the Source/Test directory.
In this file, you must name a server on which the tests can be run on. Be aware that this
should really be a test server where users, groups, etc. can be deleted and added.

The file should contain the cor api URL, username and password in on line each.
Example content:
```
demo.testhost.com/api/rest
testUser
secret_password
```
