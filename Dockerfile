# Multi-stage build para Frontend Blazor WASM
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS frontend-build
WORKDIR /app

# Copiar archivos de dependencias
COPY Delab.sln ./
COPY Delab.Shared/*.csproj ./Delab.Shared/
COPY Delab.Frontend/*.csproj ./Delab.Frontend/

# Restaurar dependencias del Frontend
RUN dotnet restore Delab.Frontend/Delab.Frontend.csproj

# Copiar código fuente del Frontend
COPY Delab.Shared/ ./Delab.Shared/
COPY Delab.Frontend/ ./Delab.Frontend/

# Publicar Frontend en Release
RUN dotnet publish Delab.Frontend/Delab.Frontend.csproj -c Release -o frontend-out

# Multi-stage build para Backend ASP.NET Core
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build
WORKDIR /app

# Copiar archivos de solución
COPY Delab.sln ./
COPY Delab.AccessData/*.csproj ./Delab.AccessData/
COPY Delab.AccessService/*.csproj ./Delab.AccessService/
COPY Delab.Helpers/*.csproj ./Delab.Helpers/
COPY Delab.Shared/*.csproj ./Delab.Shared/
COPY Delab.Backend/*.csproj ./Delab.Backend/

# Restaurar dependencias
RUN dotnet restore

# Copiar código fuente
COPY Delab.AccessData/ ./Delab.AccessData/
COPY Delab.AccessService/ ./Delab.AccessService/
COPY Delab.Helpers/ ./Delab.Helpers/
COPY Delab.Shared/ ./Delab.Shared/
COPY Delab.Backend/ ./Delab.Backend/

# Publicar Backend en Release
RUN dotnet publish Delab.Backend/Delab.Backend.csproj -c Release -o backend-out

# Runtime stage final
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copiar Backend publicado
COPY --from=backend-build /app/backend-out .

# Copiar Frontend publicado (Blazor WASM) a wwwroot
COPY --from=frontend-build /app/frontend-out/wwwroot ./wwwroot

# Configurar puertos para Backend y Frontend
# Backend API: Puerto 8080 (CapRove)
# Frontend sirve desde wwwroot del Backend
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Delab.Backend.dll"]
