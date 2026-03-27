# Použijeme oficiální .NET SDK pro sestavení aplikace
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Kopírování souborů projektu a obnova závislostí
COPY *.csproj ./
RUN dotnet restore

# Kopírování zbytku souborů a publikace
COPY . ./
RUN dotnet publish -c Release -o out

# Finální obraz pro spuštění
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Nastavení portu (Render používá port 8080 nebo 10000, ASP.NET standardně 80/8080)
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "Notes-App-C#.dll"]