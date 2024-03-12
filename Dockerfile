# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

# Install libicu package.
RUN apk add --no-cache icu-libs

COPY . /source

WORKDIR /source/EtteplanMORE.ServiceManual.UnitTests

# This is the architecture you’re building for, which is passed in by the builder.
# Placing it here allows the previous steps to be cached across architectures.
ARG TARGETARCH

# Build the application.
# Leverage a cache mount to /root/.nuget/packages so that subsequent builds don't have to re-download packages.
# If TARGETARCH is "amd64", replace it with "x64" - "x64" is .NET's canonical name for this and "amd64" doesn't
#   work in .NET 6.0.
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Run tests.
#RUN dotnet test EtteplanMORE.ServiceManual.UnitTests.csproj

WORKDIR /source/EtteplanMORE.ServiceManual.Web

# If you need to enable globalization and time zones:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/enable-globalization.md
################################################################################
# Create a new stage for running the application that contains the minimal
# runtime dependencies for the application. This often uses a different base
# image from the build stage where the necessary files are copied from the build
# stage.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Install libicu package for runtime stage.
RUN apk add --no-cache icu-libs

# Copy everything needed to run the app from the "build" stage.
COPY --from=build /app .

# Create a non-privileged user that the app will run under.
# See https://docs.docker.com/go/dockerfile-user-best-practices/
ARG UID=10001
RUN adduser \
    --disabled-password \
    --gecos "" \
    --home "/nonexistent" \
    --shell "/sbin/nologin" \
    --no-create-home \
    --uid "${UID}" \
    appuser
USER appuser

ENTRYPOINT ["dotnet", "EtteplanMORE.ServiceManual.Web.dll"]
