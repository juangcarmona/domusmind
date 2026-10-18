# Build context: repository root (e.g., /path/to/repo-root)
# docker build -f src/backend/DomusMind.Api/Dockerfile -t domusmind .

FROM node:24-alpine AS web-build
WORKDIR /src/web/app
COPY src/web/app/package.json src/web/app/package-lock.json ./
RUN npm ci
COPY src/web/app/ ./
RUN npm run build

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS backend-build
WORKDIR /src

# Copy project files first - restore layer is cached until any .csproj changes
COPY src/backend/DomusMind.Api/DomusMind.Api.csproj                         src/backend/DomusMind.Api/
COPY src/backend/DomusMind.Application/DomusMind.Application.csproj         src/backend/DomusMind.Application/
COPY src/backend/DomusMind.Contracts/DomusMind.Contracts.csproj             src/backend/DomusMind.Contracts/
COPY src/backend/DomusMind.Domain/DomusMind.Domain.csproj                   src/backend/DomusMind.Domain/
COPY src/backend/DomusMind.Infrastructure/DomusMind.Infrastructure.csproj   src/backend/DomusMind.Infrastructure/

# Copy backend source and pre-built frontend dist for publish
COPY src/backend/ src/backend/
COPY --from=web-build /src/web/app/dist/ src/web/app/dist/
RUN dotnet publish src/backend/DomusMind.Api/DomusMind.Api.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=backend-build /app/publish .

ARG DOMUSMIND_VERSION=local
ARG DOMUSMIND_COMMIT=unknown
ENV DOMUSMIND_VERSION=${DOMUSMIND_VERSION} \
    DOMUSMIND_COMMIT=${DOMUSMIND_COMMIT}

EXPOSE 8080
ENV ASPNETCORE_URLS=http://0.0.0.0:8080 \
    ASPNETCORE_ENVIRONMENT=Production

HEALTHCHECK --interval=30s --timeout=5s --start-period=60s --retries=3 \
    CMD ["bash", "-ec", "exec 3<>/dev/tcp/127.0.0.1/8080 && printf 'GET /api/system/ping HTTP/1.1\\r\\nHost: localhost\\r\\nX-Forwarded-Proto: https\\r\\nConnection: close\\r\\n\\r\\n' >&3 && IFS=' ' read -r protocol status _ <&3 && [[ \"$protocol\" == HTTP/* && \"$status\" == 200 ]]"]

ENTRYPOINT ["dotnet", "DomusMind.Api.dll"]
