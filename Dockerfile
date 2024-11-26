# Imagen base para ejecutar la API (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagen para compilar la API (SDK)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia el archivo de proyecto (.csproj) y restaura las dependencias
COPY ["PRIII-24-ESCUELA-PROGRAMACION-API/PRIII-24-ESCUELA-PROGRAMACION-API.csproj", "PRIII-24-ESCUELA-PROGRAMACION-API/"]
RUN dotnet restore "PRIII-24-ESCUELA-PROGRAMACION-API/PRIII-24-ESCUELA-PROGRAMACION-API.csproj"

# Copia todo el código fuente y compila el proyecto
COPY . .
WORKDIR "/src/PRIII-24-ESCUELA-PROGRAMACION-API"
RUN dotnet build "PRIII-24-ESCUELA-PROGRAMACION-API.csproj" -c Release -o /app/build

# Publica la aplicación
FROM build AS publish
RUN dotnet publish "PRIII-24-ESCUELA-PROGRAMACION-API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final con la API publicada lista para ejecutarse
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRIII-24-ESCUELA-PROGRAMACION-API.dll"]
