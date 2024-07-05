	#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TaxCalculator.Presentation.Api/TaxCalculator.Presentation.Api.csproj", "TaxCalculator.Presentation.Api/"]
COPY ["TaxCalculator.Persistance/TaxCalculator.Persistance.csproj", "TaxCalculator.Persistance/"]
COPY ["TaxCalculator.Application/TaxCalculator.Application.csproj", "TaxCalculator.Application/"]
COPY ["TaxCalculator.Domain/TaxCalculator.Domain.csproj", "TaxCalculator.Domain/"]
RUN dotnet restore "./TaxCalculator.Presentation.Api/TaxCalculator.Presentation.Api.csproj"
COPY . .
WORKDIR "/src/TaxCalculator.Presentation.Api"
RUN dotnet build "./TaxCalculator.Presentation.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TaxCalculator.Presentation.Api.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TaxCalculator.Presentation.Api.dll"]