#! /bin/sh

CONF=Release

dotnet clean -c ${CONF}
dotnet build -c ${CONF}
dotnet tarball -c ${CONF}

