FROM mcr.microsoft.com/dotnet/sdk:5.0.405

ENV ASPNETCORE_ENVIRONMENT=${TARGET_ENV}

WORKDIR /app

# Copy all files excluding ones in .dockerignore
COPY . .

# Restore and build all subprojects
RUN dotnet restore
RUN dotnet build --no-restore

# Configure developer certificate
RUN dotnet dev-certs https

# Install tools for database migration
RUN dotnet tool install dotnet-ef --global

# Add installed tools to PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Migrate and update database
RUN dotnet ef migrations --project src/DevSpector.Database --startup-project src/DevSpector.UI add DockerInit --no-build
RUN dotnet ef database --project src/DevSpector.Database --startup-project src/DevSpector.UI update

# Run entry subproject when deployed
CMD dotnet run --project src/DevSpector.UI --launch-profile ${TARGET_ENV}
