FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# نسخ ملف المشروع وعمل restore
COPY ["CrochetStore.csproj", "./"]
RUN dotnet restore "CrochetStore.csproj"

# نسخ باقي الملفات وبناء المشروع
COPY . .
RUN dotnet publish "CrochetStore.csproj" -c Release -o /app/publish /p:UseAppHost=false

# المرحلة النهائية للتشغيل
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CrochetStore.dll"]