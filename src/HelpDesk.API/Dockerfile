FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HelpDesk.API/HelpDesk.API.csproj", "HelpDesk.API/"]
COPY ["HelpDesk.Application/HelpDesk.Application.csproj", "HelpDesk.Application/"]
COPY ["HelpDesk.Domain/HelpDesk.Domain.csproj", "HelpDesk.Domain/"]
COPY ["HelpDesk.Infrastructure/HelpDesk.Infrastructure.csproj", "HelpDesk.Infrastructure/"]

RUN dotnet restore "HelpDesk.API/HelpDesk.API.csproj"

COPY . .

WORKDIR /src/HelpDesk.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "HelpDesk.API.dll"]