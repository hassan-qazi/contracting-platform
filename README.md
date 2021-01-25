# contracting-platform

ASP.NET Core MVC Application 

If you have .net core on your machine:

- Clone the repo
- cd ContractingPlatform
- dotnet run

If you prefer to use docker instead:

- Clone the repo
- cd ContractingPlatform
- Edit Properties\launchSettings.json, line 22, "applicationUrl": "https://localhost:5001;http://localhost:5000" to "applicationUrl": "https://*:5001;http://*:5000"
- docker build -t cp-image .
- docker run -it --rm -p 8080:5001 --name cp-app cp-image
- Open browser and access on port 8080 (localhost:8080 for linux; <container-ip>:8080 for windows)
- If you recieve a cert error on the browser, click Advanced and Proceed to app
  
