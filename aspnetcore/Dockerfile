FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 63293
EXPOSE 44357

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["aspnetcore/aspnetcore.csproj", "aspnetcore/"]
RUN dotnet restore "aspnetcore/aspnetcore.csproj"
COPY . .
WORKDIR "/src/aspnetcore"
RUN dotnet build "aspnetcore.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "aspnetcore.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "aspnetcore.dll"]