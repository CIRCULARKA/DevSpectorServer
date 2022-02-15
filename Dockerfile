FROM mcr.microsoft.com/dotnet/sdk:5.0.405

WORKDIR /src

COPY src .

RUN dotnet tool install dotnet-ef --global

RUN dotnet dev-certs https

ENV PATH="${PATH}:/root/.dotnet/tools"

RUN dotnet ef migrations --project DevSpector.Database --startup-project DevSpector.UI add Init
RUN dotnet ef database --project DevSpector.Database --startup-project DevSpector.UI update

CMD [ "dotnet", "run", "--project", "DevSpector.UI", "--launch-profile", "Default" ]

EXPOSE 5005

# [Optional]
# This command allow to setup environment variables
# As it completes you can access PORT variable from running application
# and get value of 5040
ENV PORT=5040
