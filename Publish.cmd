SET configuration=Release
SET out=C:/Publish

call dotnet pack ./src/EdjCase.BasicAuth -c %configuration% -o "%out%/EdjCase.BasicAuth"