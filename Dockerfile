FROM mcr.microsoft.com/dotnet/sdk:5.0.405

WORKDIR /app

EXPOSE 5000

COPY DevSpectorServer.sln .
COPY src src
COPY tests tests

RUN dotnet restore
RUN dotnet build --no-restore

RUN dotnet dev-certs https

RUN dotnet tool install dotnet-ef --global

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet ef migrations --project src/DevSpector.Database --startup-project src/DevSpector.UI add DockerInit
RUN dotnet ef database --project src/DevSpector.Database --startup-project src/DevSpector.UI update DockerInit --connection $CON_STR

RUN dotnet publish src/DevSpector.UI -c Release -r linux-x64 -o publish

RUN cp src/DevSpector.UI/Data.db publish/Data.db

CMD ASPNETCORE_URLS=http://+:${PORT} ./publish/DevSpector.UI
