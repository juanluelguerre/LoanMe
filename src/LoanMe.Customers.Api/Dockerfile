FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["LoanMe.Api/LoanMe.Api.csproj", "LoanMe.Api/"]
RUN dotnet restore "LoanMe.Api/LoanMe.Api.csproj"
COPY . .
WORKDIR "/src/LoanMe.Api"
RUN dotnet build "LoanMe.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LoanMe.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LoanMe.Api.dll"]
