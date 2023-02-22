
REM make network_local.bat
REM "<path>protoc.exe" network.proto --csharp_out=.
.\Tools\protoc.exe -I=Protos\. --csharp_out=. .\Protos\*.proto
.\Tools\protoc.exe -I=Rpcs\. --csharp_out=. --plugin=protoc-gen-grpc=.\Tools\grpc_csharp_plugin.exe .\Rpcs\*.proto

REM pause