FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
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
ENTRYPOINT ["dotnet", "ComputerVisionService.dll"]
