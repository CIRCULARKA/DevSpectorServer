FROM mcr.microsoft.com/dotnet/sdk:5.0.405

# ENV ASPNETCORE_ENVIRONMENT=${TARGET_ENV}
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
RUN if [ "${TARGET_ENV}" = "Development" ] ; then dotnet ef migrations --project src/DevSpector.Database --startup-project src/DevSpector.UI add DockerInit --no-build ; echo "Migration skipped" ; fi
RUN if [ "${TARGET_ENV}" = "Development" ] ; then dotnet ef database --project src/DevSpector.Database --startup-project src/DevSpector.UI update ; echo "Database update skipped" ; fi

# Run entry subproject when deployed
CMD dotnet run --project src/DevSpector.UI --launch-profile ${TARGET_ENV}
