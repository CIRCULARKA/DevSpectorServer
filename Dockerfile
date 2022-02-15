FROM mcr.microsoft.com/dotnet/sdk:5.0.405

WORKDIR /src

COPY src .

RUN dotnet tool install dotnet-ef --global

RUN dotnet dev-certs https

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet ef migrations --project DevSpector.Database --startup-project DevSpector.UI add Init
RUN dotnet ef database --project DevSpector.Database --startup-project DevSpector.UI update

EXPOSE 5005

CMD [ "dotnet", "run", "--project", "DevSpector.UI", "--launch-profile", "Default" ]
