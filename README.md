# For developers
```Appliance``` entity is simplyfied client side copy of ```Device``` class. It is made like that in order to resolve ambiguity between ```Xamarin.Forms.Device``` and our own implementation of this class

# TODO
## Desktop
- Create something like session broker - the line on the top of the program that indicates state of connection,
logged user login and group with logout button
- Implement all server features (in progress)
- Make main menu button in ```ModernMenu.cs``` have own icon resource inside the style
- Resolve all temporary solutions: ```SoftwareInfoViewModel.cs``` in ```UpdateDeviceInfo``` method
- Resolve weird bug at ```ModernMenu.cs``` in ```OnInitialized``` method
-
- Automate the process of subscribtion view models to app events (done halfway)
- Add authorization view (done)
- Make UI display following sections: common information, network information, software and location of device (done)

## Server
- Modifying devices: add and delete
- Account devices software
- Account devices credentials
- Assign IP addresses to devices
- Manage users roles (done)
- Add access only via API key (done)
- Add authorization (done)
