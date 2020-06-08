# ThermalPrint
This project allows you to print an invoice after receive a JSON document.
## Functionality
The Web API was developed in **.Net framework 4.6.1** with cors support to allow other applications execute itspublic function **send** in the printcontroller without any issue. The main idea is call it using the localhost address.

To see the configuration, goto `<remoteaddress>`/swagger/ to see the functions.

## Fix Errors in the web API.
In some computers we can receive an error related to access permissions Error writing file report: Access to the path 'c:\windows\system32\inetsrv\Ofidental\202006081051084_c62928f-aec5-4344-8081-4453ebf573fe.txt' is denied., please validate the ApplicationHost.Config will have the following configuration to access to the user profile:

`<applicationPoolDefaults managedRuntimeVersion="v4.0">
  <processModel identityType="ApplicationPoolIdentity" loadUserProfile="true" setProfileEnvironment="true" />
</applicationPoolDefaults>`

Location: C:\Windows\System32\inetsrv\config\applicationHost.config
