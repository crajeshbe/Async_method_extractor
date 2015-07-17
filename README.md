# Async_method_extractor
Extract Async Methods from .NetFramework BCL

It is quite difficult to read meta-data of Dotnet framework B(F)CL libraries using Dotnet reflection. 

In this project, I've used Mono cecil to read and extract all Async methods from Dotnet framework B(F)CL libraries. This application takes couple of argument.

1. Location of Dotnet framework B(F)CL libraries 
2. Location to save the extracted method name (with file name)