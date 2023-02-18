FROM fanto/ubuntu20.04-opencvsharp-lib:1.01 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["ComputerVisionService/ComputerVisionService.csproj", "ComputerVisionService/"]
COPY ["SharedLib/SharedLib.csproj", "SharedLib/"]
COPY . .
RUN dotnet restore "ComputerVisionService/ComputerVisionService.csproj"  --disable-parallel
WORKDIR "/src/ComputerVisionService"
RUN dotnet build "ComputerVisionService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComputerVisionService.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=build /src/ComputerVisionService/Cascades/haarcascade_frontalface_alt.xml ./Cascades/
ENTRYPOINT ["dotnet", "ComputerVisionService.dll"]
