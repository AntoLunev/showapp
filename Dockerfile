FROM ubuntu:22.04

ENV SOURCEDIR /source
ENV APPDIR /app

# install the .NET 6 SDK from the Ubuntu archive
# (no need to clean the apt cache as this is an unpublished stage)
RUN apt-get update && apt-get install -y dotnet6 ca-certificates
RUN groupadd -g 1001 jenkinsbuild

WORKDIR $APPDIR
RUN touch version.txt

# add your application code
WORKDIR $SOURCEDIR
# using all files from this folder as a source
COPY . .

RUN dotnet build ./showapp/
RUN dotnet publish ./showapp/ -c Release -r ubuntu.22.04-x64 --self-contained true -o $APPDIR

# Cleanup and change group owner to jenkinsbuild to allow testing
RUN rm -rf ./showapp/obj && rm -rf ./showapp/bin && rm -rf /tmp/NuGetScratch && chown -R :jenkinsbuild $SOURCEDIR
