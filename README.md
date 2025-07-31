# vworks-atc-plugin
VWorks plugin for Thermofisher ATC

ATC Connection Notes:
- On startup, the ATC looks for a DHCP server to assign it an IP address.  If no DHCP is present, it will assign itself a (random?) IP address, usually on 169.x.x.x.  So, either install a DHCP server on the host PC, or change the adapter settings to be automatically assigned IP/Gateway/DNS.

### Notes on compiling / loading into VWorks
- DLL needs to be placed in C:\Program Files (x86)\Agilent Technologies\VWorks\Plugins (along w/ tlb and pdb if debugging)
- Can use IWorksTest.exe (in VWorks folder) to test basic functions.
- Everything needs to be x86

#### Testing in IWorksTest
To test it in IWorksTests, you must include the dll, tlb, and pdb files otherwise it won't show up in the list of plugins.

To generate the `.tlb` file, you will first need to run Visual Studio as an administrator. Then you'll need to go into the project settings by opening the solutions file (`.sln` file) and going into the project properties and going into `Project > VWorks Mantis Driver Properties > Build` and checking the "Register for COM interop" checkbox. If your project is already built, you'll have to clean and rebuild as Visual Studio isn't smart enough to rebuild.
