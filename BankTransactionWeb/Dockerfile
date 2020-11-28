#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BankTransactionWeb/BankTransaction.Web.csproj", "BankTransactionWeb/"]
COPY ["BankTransaction.Entities/BankTransaction.Entities.csproj", "BankTransaction.Entities/"]
COPY ["BankTransaction.Configuration/BankTransaction.Configuration.csproj", "BankTransaction.Configuration/"]
COPY ["BankTransaction.Models/BankTransaction.Models.csproj", "BankTransaction.Models/"]
COPY ["BankTransactionWeb.BAL/BankTransaction.BAL.Implementation.csproj", "BankTransactionWeb.BAL/"]
COPY ["BankTransaction.Api.Models/BankTransaction.Api.Models.csproj", "BankTransaction.Api.Models/"]
COPY ["BankTransaction.DAL.Abstract/BankTransaction.DAL.Abstract.csproj", "BankTransaction.DAL.Abstract/"]
COPY ["BankTransaction.BAL.Abstract/BankTransaction.BAL.Abstract.csproj", "BankTransaction.BAL.Abstract/"]
COPY ["BAnkTransactionWeb.DAL/BankTransaction.DAL.Implementation.csproj", "BAnkTransactionWeb.DAL/"]
COPY ["BankTransaction.Web.Localization/BankTransaction.Web.Localization.csproj", "BankTransaction.Web.Localization/"]
RUN dotnet restore "BankTransactionWeb/BankTransaction.Web.csproj"
COPY . .
WORKDIR "/src/BankTransactionWeb"
RUN dotnet build "BankTransaction.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BankTransaction.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BankTransaction.Web.dll"]