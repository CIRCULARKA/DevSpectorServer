# Inherit this image from Microsoft's one which is installing to
# container dotnet SDK of specified below version
FROM mcr.microsoft.com/dotnet/sdk:5.0.405

# Set working directory for current context
# Creates the "src" folder and "cd" to it
WORKDIR /src

# Copy all files relative to this Dockerfile (recursively) and put
# them into /src folder (as this folder is CWD now)
COPY . .

# COPY . /src # this is how it'll look like if WORKDIR command wasn't used

# This command is used to run things on image build. So it is executes only one time
# Because of this, this command is useful to setup environment, i.e. download tools
# from the internet or something like this and we do not want to do it each run of
# container
# RUN dotnet tools --install dotnet-ef --global


# CMD launches commands below each run of the container
# so it is useful for starting application each container run which is logical
CMD [ "dotnet", "run", "--project", "Server/InvMan.Server.UI" ]

# [Optional]
# Set kind of PORT environment variable that can be used in application
# so the webapp could be started on specified port
# I don't know how to get this value from app
# (I searched in PORT variable but nothing there)
# EXPOSE 5040

# [Optional]
# This command allow to setup environment variables
# As it completes you can access PORT variable from running application
# and get value of 5040
ENV PORT=5040
