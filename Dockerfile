# Sestavení aplikace
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Kopírujeme úplně všechno z tvého GitHubu do Dockeru
COPY . ./

# Najdeme .csproj a obnovíme závislosti (i když je v podsložce)
RUN dotnet restore

# Publikujeme aplikaci (všimni si tečky na konci - říká "tady v té složce")
RUN dotnet publish -c Release -o out

# Finální obraz
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Port pro Render
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Tady POZOR: Musí to být přesný název tvého souboru
ENTRYPOINT ["dotnet", "Notes-App-C#.dll"]