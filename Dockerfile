FROM mcr.microsoft.com/dotnet/sdk:6.0.300

# ENV ASPNETCORE_ENVIRONMENT=${TARGET_ENV}
ARG TARGET_ENV
ENV ASPNETCORE_ENVIRONMENT=${TARGET_ENV}

# Use "app" as workdir root
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

# Run entry subproject when deployed
CMD ASPNETCORE_ENVIRONMENT=${TARGET_ENV} && [ "${TARGET_ENV}" = "Development" ] && dotnet ef migrations --project src/DevSpector.Database --startup-project src/DevSpector.UI add DockerInit && dotnet run --project src/DevSpector.UI --launch-profile ${TARGET_ENV}
