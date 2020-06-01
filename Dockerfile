
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app

COPY *.sln ./
COPY */*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*} && mv $file ${file%.*}/; done
RUN dotnet restore

COPY . ./
RUN cd Enxaquecapp.WebApi && dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/Enxaquecapp.WebApi/out .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet Enxaquecapp.WebApi.dll