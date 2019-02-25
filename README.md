# PolyMicron blog engine

* Setup dotnet runtime/sdk
* Setup postgres
* create database pmdb;
* cd Pm.Data 
* dotnet ef database update;
* cd PolyMicron project
* Edit MainLayout.cshtml for custom header and footer
* Edit Home/About.cshtml for custom about page
* open appsettings.Development.json and appsettings.Production.json
* Change conn string, **CHANGE ROOT CREDS**, specify GoogleReCaptcha keys
* Edit MainLayout.cshtml for custom header and footer
* Edit Home/About.cshtml for custom about
* dotnet run (development run) or setup nginx and use .service file to make prod server
* browse to /cms, log in, and create posts (then publish in post list)

![blog post](https://i.postimg.cc/Dwz0Rcbh/blog-alon.png)

[![dash.png](https://i.postimg.cc/G2tj2DNn/dash.png)](https://postimg.cc/2bpW91r0)