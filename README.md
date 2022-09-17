# File Distribution Service (FDS)
The solution uses the Entity Framework Core Code First Migrations. It comes with a .DbMigrator console application which applies the migrations and also seeds the initial data.

.DbMigrator project has its own appsettings.json. So, if you have changed the connection string above, you should also change this one.

# The Initial Migration
.DbMigrator application automatically creates the Initial migration on first run.

If you are using Visual Studio, you can skip to the Running the DbMigrator section. However, other IDEs (e.g. Rider) may have problems for the first run since it adds the initial migration and compiles the project. In this case, open a command line terminal in the folder of the .DbMigrator project and run the following command:
dotnet run
Some basic Git commands are:
```
dotnet run
```
For the next time, you can just run it in your IDE as you normally do
**Running the DbMigrator**
Right click to the .DbMigrator project and select Set as StartUp Project
```
Initial seed data creates the admin user in the database (with the password is 1q2w3E*) and 
client user (with the password is Pop@12345)  which is then used to login to the application. So, you need to use .DbMigrator at least once for a new database.
```
# Run the Applications
Run The command on your solution project folder to install styles for  FDSService.IdentityServer project
```
Run abp install-libs 
```

# Running the FDSService.HttpApi.Host && FDSService.IdentityServer (Server Side)
Right click solution explorer go to properties and select option **Multiple start projects** select the project  FDSService.HttpApi.Host and FDSService.IdentityServer then press apply
# Running the Angular Application (Client Side)
Go to the angular folder, open a command line terminal, type the yarn command (we suggest to the yarn package manager while npm install will also work)
```
yarn
```
Once all node modules are loaded, execute yarn start (or npm start) command:
```
yarn start
```
It may take a longer time for the first build. Once it finishes, it opens the Angular UI in your default browser with the localhost:4200 address.

## Note:
If get error to install angular application please run the following command
```
npm install --legacy-peer-deps
```
