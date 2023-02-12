FROM mcr.microsoft.com/dotnet/runtime:6.0-focal AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["ComputerVisionService/ComputerVisionService.csproj", "ComputerVisionService/"]
COPY ["SharedLib/SharedLib.csproj", "SharedLib/"]
RUN dotnet restore "ComputerVisionService/ComputerVisionService.csproj"
COPY . .
WORKDIR "/src/ComputerVisionService"
RUN dotnet build "ComputerVisionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComputerVisionService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN apt-get update && apt-get install \
          libjasper1 \
          libtiff-dev \
          libavcodec-dev \
          libavformat-dev \
          libswscale-dev \
          libxine2-dev \
          libv4l-dev
ENTRYPOINT ["dotnet", "ComputerVisionService.dll"]
