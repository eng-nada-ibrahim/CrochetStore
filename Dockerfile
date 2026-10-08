FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CrochetStore.csproj", "./"]
RUN dotnet restore "CrochetStore.csproj"
COPY . .
RUN dotnet publish "CrochetStore.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CrochetStore.dll"]