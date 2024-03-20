Scaffold out a controller from a model:

dotnet aspnet-codegenerator controller -name <Model>sController -m <Model> -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries

dotnet aspnet-codegenerator identity -dc JobWebsiteMVC.Data.ApplicationDbContext --files "Account.Manage.Index;"

marcus_stockton@hotmail.co.uk
Test@test.com
P@55w0rd!


Free postcode lookup stuff: 
http://api.getthedata.com/postcode/WC1A+1AB


Claim Types:
- JobSeekerOnly
- JobOwnerOnly
- AdminOnly


Wire in SignalR to update the admin panel when stuff happens (new users, new jobs etc)

Added markdown (https://github.com/xoofx/markdig) - implement with preview when creating/editing jobs