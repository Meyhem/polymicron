# PolyMicron blog engine

1. Setup dotnet runtime/sdk
2. Setup postgres
3. create database pmdb;
4. cd Pm.Data 
5. appsettings.json -> your conn string
6. dotnet ef database update; 
7. cd ../PolyMicron
8. open appsettings.Development.json and appsettings.Production.json
9. Change conn string, **CHANGE ROOT CREDS**, specify GoogleReCaptcha keys
10. Edit MainLayout.cshtml for custom header and footer
11. Edit Home/About.cshtml for custom about
12. dotnet run (development run) or setup nginx and use .service file to make prod server
