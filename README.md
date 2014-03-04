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


Using Pash
==========
Pash is an open source implementation of Powershell that works on many platforms, including Unix/Linux and Mac.
Pash is still in the pre-alpha phase and not very stable, yet. However, one goal of this project is that
the cmdlets are also usable with Pash. Indeed, some basic operations already work.

Loading the compiled module
---------------------------
So far, Pash only supports PSSnapins, but in contrast to Powershell, it is also to load PSSnapins via file in Pash, so
an installation of the PSSnapin is not needed (and in fact not supported at the moment).
Simply load the PSSnapIn by calling
```
Add-PSSnapin /path/to/OpenDataSpace.Commands.dll
```
Make sure this path starts with a slash or is written as a relative path (e.g. "./OpenDataSpace.Commands.dll").

Request errors
--------------
If you're encountering request errors that contain something about "trust", "decryption", etc, than you need to import
some certificates for mono. This can be done e.g. by calling
```
mozroots --import
```
This is a known issue in mono and discussed for example at http://www.mono-project.com/FAQ:_Security.
It is only necessary to run this command once, not every time the cmdlets are used.

Known issues
------------
At the moment, Pash doesn't support parameter values from object by name, therefore many pipeline operations currently
don't work with Pash.
Also, Pash currently won't ask you for mandatory parameters that you haven't provided, but throw an error instead.

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
	