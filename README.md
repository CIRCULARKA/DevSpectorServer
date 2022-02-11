# For developers
```Appliance``` entity is simplyfied client side copy of ```Device``` class. It is made like that in order to resolve ambiguity between ```Device``` from server application layer and SDK's ```Device```.

# TODO
For TODO list, bug list and tasks in progress head [here](https://github.com/CIRCULARKA/InvMan/projects/1)

# Build
Basically, there are desktop client and server apps. So to use desktop app you must start server locally. To do this run:

```dotnet run --project <root-of-the-project>/Server/InvMan.Server.UI```

Then you can run desktop client:

```dotnet run --project <root-of-the-project>/Desktop/InvMan.Desktop.UI```
