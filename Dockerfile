FROM microsoft/aspnetcore-build:1.1 AS builder

WORKDIR /source
RUN mkdir Diporto \
  && mkdir DiportoTests
COPY Diporto/*.csproj ./Diporto
COPY DiportoTests/*.csproj ./DiportoTests
RUN cd Diporto \
  && dotnet restore \
  && cd ../DiportoTests \
  && dotnet restore \
  && cd ..

COPY . .
RUN npm install -g yarn

WORKDIR /source/Diporto
RUN dotnet publish Diporto.csproj --output /app/ --configuration Release

FROM microsoft/aspnetcore:1.1
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "Diporto.dll"]
