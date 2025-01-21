# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia los archivos de proyecto en el orden correcto
COPY ["Core/Core.csproj", "./Core/"]
COPY ["Application/Application.csproj", "./Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "./Infrastructure/"]
COPY ["WebApiPersonas/WebApiPersonas/WebApiPersonas.csproj", "./WebApiPersonas/"]

# Restaurar las dependencias desde el proyecto principal
RUN dotnet restore "WebApiPersonas/WebApiPersonas.csproj"

# Copiar todo el contenido al contenedor
COPY . .

# Construir el proyecto principal
WORKDIR "/src/"
RUN dotnet build "WebApiPersonas/WebApiPersonas/WebApiPersonas.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "WebApiPersonas/WebApiPersonas/WebApiPersonas.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiPersonas.dll"]
