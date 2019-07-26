FROM microsoft/dotnet:2.2-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ms-amqp/ms-amqp.Core.csproj ms-amqp/
RUN dotnet restore ms-amqp/ms-amqp.Core.csproj
COPY . .
WORKDIR /src/ms-amqp
RUN dotnet build ms-amqp.Core.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ms-amqp.Core.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ms-amqp.Core.dll"]

# Set the locale
ENV TZ=America/Sao_Paulo
ENV LANG pt-BR
ENV LANGUAGE pt-BR
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
