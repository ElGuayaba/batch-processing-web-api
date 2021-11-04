FROM mcr.microsoft.com/mssql/server:2017-latest

ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=abcDEF123#
ENV MSSQL_PID=Developer

WORKDIR /src

COPY filldata.sql ./filldata.sql

RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" &&  /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P abcDEF123# -i filldata.sql