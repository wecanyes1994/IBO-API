# 使用 .NET SDK 建置
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# 複製 csproj 並還原套件
COPY *.csproj ./
RUN dotnet restore

# 複製所有原始碼並建置 Release
COPY . ./
RUN dotnet publish -c Release -o out

# 使用 ASP.NET Runtime 執行
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# 對外開放埠口（Render 預設會使用 $PORT）
ENV ASPNETCORE_URLS=http://+:$PORT
ENTRYPOINT ["dotnet", "IBOWebAPI.dll"]
