# thermalprint
This project allows you to print an invoice after receive a JSON document.

In some computers we can receive an error related to access permissions, please validate the ApplicationHost.Config will have the following configuration to access to the user profile:

<applicationPoolDefaults managedRuntimeVersion="v4.0">
  <processModel identityType="ApplicationPoolIdentity" loadUserProfile="true" setProfileEnvironment="true" />
</applicationPoolDefaults>
