
FROM microsoft/dotnet:2.1-sdk
WORKDIR /src
COPY ["PayPalWebHookListener.Api/PayPalWebHookListener.Api.csproj", "PayPalWebHookListener.Api/"]
RUN dotnet restore "PayPalWebHookListener.Api/PayPalWebHookListener.Api.csproj"
COPY . .
WORKDIR "/src/PayPalWebHookListener.Api"
RUN dotnet publish "PayPalWebHookListener.Api.csproj" -c Release -o /app
ENTRYPOINT ["dotnet", "/app/PayPalWebHookListener.Api.dll"]