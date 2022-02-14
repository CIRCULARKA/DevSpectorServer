server:
	dotnet run --project Server/InvMan.Server.UI -v q
desktop:
	dotnet run --project Desktop/InvMan.Desktop.UI -v q
build-sdk:
	dotnet build Common/InvMan.Common.SDK -v q
server-tests:
	dotnet test Tests/Tests.Server -v m
sdk-tests:
	dotnet test Tests/Tests.Common -v m
run-tests: server-tests sdk-tests
