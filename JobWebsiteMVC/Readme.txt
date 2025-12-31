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
Consider adding in https://github.com/jefago/tiny-markdown-editor for markdown editing


``docker build -t flaskpropertymanager .``\
``docker run -it -p 5000:5000 flaskpropertymanager``\
``docker exec -it <container name> bash`` # to load up the docker image to navigate in linux\
``docker compose build`` # builds all images\
``docker compose build flask_app`` # Builds specific image\
``docker compose up -d`` # runs all images in detached mode\
``docker compose up -d --build``
``docker compose up -d flask_app`` # runs one particular image in detached mode