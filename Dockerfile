FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# 1. Zkopírujeme všechno do kontejneru
COPY . .

# 2. Vstoupíme do podsložky s projektem (TADY ZMĚŇ NÁZEV, POKUD SE SLOŽKA JMENUJE JINAK)
WORKDIR "/src/Notes-App-C#/Notes-App-C#"

# 3. Obnovíme a publikujeme přímo ze složky projektu
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# 4. Finální obraz
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Tady POZOR: Musí to být přesný název tvého DLL
ENTRYPOINT ["dotnet", "Notes-App-C#.dll"]