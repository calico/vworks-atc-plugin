# vworks-atc-plugin
VWorks plugin for Thermofisher ATC

ATC Connection Notes:
- On startup, the ATC looks for a DHCP server to assign it an IP address.  If no DHCP is present, it will assign itself a (random?) IP address, usually on 169.x.x.x.  So, either install a DHCP server on the host PC, or change the adapter settings to be automatically assigned IP/Gateway/DNS.  
