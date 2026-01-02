# Build stage: Use .NET 8 SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore dependencies
COPY ["HouseHoldeBudgetApi.csproj", "."]
RUN dotnet restore "HouseHoldeBudgetApi.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src"
RUN dotnet build "HouseHoldeBudgetApi.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "HouseHoldeBudgetApi.csproj" -c Release -o /app/publish

# Runtime stage: Use ASP.NET Core runtime to run the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user for security
RUN adduser --disabled-password --home /app --uid 1000 --gecos "" appuser && \
    chown -R appuser:appuser /app
USER appuser

# Copy from build stage
COPY --from=build /app/publish .

# Expose ports
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80;https://+:443
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "HouseHoldeBudgetApi.dll"]