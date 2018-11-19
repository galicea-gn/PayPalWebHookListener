
FROM microsoft/dotnet:2.1-sdk
WORKDIR /src
COPY ["RevolutionGolf.PayPalListener/RevolutionGolf.PayPalListener.csproj", "RevolutionGolf.PayPalListener/"]
RUN dotnet restore "RevolutionGolf.PayPalListener/RevolutionGolf.PayPalListener.csproj"
COPY . .
WORKDIR "/src/RevolutionGolf.PayPalListener"
RUN dotnet publish "RevolutionGolf.PayPalListener.csproj" -c Release -o /app
ENTRYPOINT ["dotnet", "/app/RevolutionGolf.PayPalListener.dll"]