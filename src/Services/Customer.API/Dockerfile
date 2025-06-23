FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

#Copy các project.csproj reference đến Project Customer.API
COPY ["Services/Customer.API/Customer.API.csproj","Services/Customer.API/"]
COPY ["BuildingBlocks/Common.Logging/Common.Logging.csproj","BuildingBlocks/Common.Logging/"]
COPY ["BuildingBlocks/Contracts/Contracts.csproj","BuildingBlocks/Contracts/"]
COPY ["BuildingBlocks/Infrastructure/Infrastructure.csproj","BuildingBlocks/Infrastructure/"]
COPY ["BuildingBlocks/Shared/Shared.csproj","BuildingBlocks/Shared/"]

RUN dotnet restore "Services/Customer.API/Customer.API.csproj"
COPY . .
WORKDIR "/src/Services/Customer.API"
RUN dotnet build "Customer.API.csproj" -c release -o app/build

FROM build as publish
RUN dotnet publish "Customer.API.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /APP
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet","Customer.API.dll"]