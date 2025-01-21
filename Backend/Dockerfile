# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copia los archivos de proyecto en el orden correcto
COPY ["Core/Core.csproj", "Core/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["WebApiPersonas/WebApiPersonas/WebApiPersonas.csproj", "WebApiPersonas/"]

# Verificar que los archivos .csproj están en las rutas correctas
RUN echo "Contenido de /src después de copiar archivos .csproj:" && ls -R /src

# Restaurar las dependencias desde el proyecto principal
RUN dotnet restore "WebApiPersonas/WebApiPersonas.csproj"

# Verificar dependencias restauradas
RUN echo "Contenido de /src después de restaurar dependencias:" && ls -R /src

# Copiar todo el contenido al contenedor
COPY . .

# Verificar contenido después de copiar todo
RUN echo "Contenido de /src después de copiar todo el proyecto:" && ls -R /src

# Construir el proyecto principal
WORKDIR "/src/"
RUN dotnet build "WebApiPersonas/WebApiPersonas/WebApiPersonas.csproj" -c Release -o /app/build

# Verificar contenido del directorio de construcción
RUN echo "Contenido de /app/build después de construir el proyecto:" && ls -R /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "WebApiPersonas/WebApiPersonas/WebApiPersonas.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Verificar contenido del directorio de publicación
RUN echo "Contenido de /app/publish después de publicar el proyecto:" && ls -R /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Verificar contenido final en /app
RUN echo "Contenido de /app en la imagen final:" && ls -R /app

ENTRYPOINT ["dotnet", "WebApiPersonas.dll"]