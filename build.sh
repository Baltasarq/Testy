dotnet build -c:Release
ilrepack /out:dist/Testy.dll bin/Release/net10.0/Testy.dll bin/Release/net10.0/*.dll
