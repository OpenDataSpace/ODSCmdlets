Development and Testing
=======================
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




Adding the Cmdlets to a PowerShell Runspace
===========================================
The compiled binary can be added in two ways to the PowerShell session: as a module or as a PSSnapIn.

Module: The easy way
--------------------
If you are using PowerShell 2.0 or higher, you should import the binary as a module. This can be done
with the following command:
```
Import-Module C:\path\to\the\binary\OpenDataSpace.Commands.dll
```

PSSnapIn: The tedious way
-------------------------
If you are using an older version of PowerShell, you can also add the cmdlets to you PowerShell runspace by
 nstalling the binary and adding it as a PSSnapIn.
To install the binary, you need to run the "InstallUtil" with the binary as argument.
Depending on your installation, it could be something like
```
"C:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe" path\to\OpenDataSpace.Commands.dll
```
Aftwerwards, you can simply add the PSSnapIn (and therefore the ODS cmdlets) to your PowerShell session by calling
```
Add-PSSnapIn ODSCmdlets
```
After this command executed successfully, you can start using all the commands.




Example Cmdlet Usage
====================
After adding the SnapIn, you need to connect to your DataSpace, e.g. by calling:
```
Connect-ODS -URL <your url to the dataspace api> -UserName <username> -Password <password>
```
The dataspace API URL is usually <hostname>/api/rest.

If this command executed without any errors, you can use all other cmdlets. E.g. you can add a user via:
```
Add-ODSUser -UserName <username> -FamilyName <lastname> -FirstName <firstname> -EMail <email adddress>
	-Language <language identifier> -Role <role name> -MaxSpace <maximal space in bytes>
```
This command would return an object on success, representing the created user.
Note that all these values can also be piped to the cmdlet. For example:
```
Import-CSV "new_users.csv" | Add-ODSUser -Role "DataSpaceDefaultUser" -MaxSpace [math]::pow(1024, 3)
```
would add a DataSpace user for all users in the csv file, provided that the fields are named correctly.
Each added user would be a DataSpaceDefaultUser and have 1GB max space.

You can pipe various ODS cmdlets, e.g. to easily remove a user by username:
```
Get-ODSUser -Name <username> -Exact | Remove-ODSUser
```
The first cmdlet would get the one user who hase exactly the provided username and remove it from the DataSpace.