# CS460-Fall24-ClassRepo
## Miscellaneous
.NET 8.0 is the current "main" release.  Let's use that one.
```
dotnet --list-sdks
dotnet help
dotnet new -h
dotnet help new
```

## Starting a Homework with a main project, a testing project and a solution file to hold them
Use this to start all your homeworks.

Creates (or uses) a folder named Sample, with subfolders Sample and Sample_Tests.  Places a solution named "Sample"  in the outer folder, a MVC project named Sample in the Sample/Sample folder and a NUnit project in the Sample/Sample_Tests folder.  Adds a reference from the testing project to the main project so it can be used to test code in the main project.
```
dotnet new mvc --output Sample1/Sample2 --framework net8.0 --auth None --use-program-main true
dotnet new sln -o Sample1
dotnet sln Sample1 add Sample1/Sample2
dotnet new nunit --output Sample1/Sample_Tests --framework net8.0
dotnet sln Sample1 add Sample1/Sample_Tests
dotnet add Sample1/Sample_Tests reference Sample1/Sample2
```

## Add required packages
[Documentation for command](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-add-package)
[NuGet Repository](https://www.nuget.org/)
Either manually add version information (--version) or double check that you got the ones you wanted

```
#cd into the project you want to add to, i.e. Sample1/Sample2 (so you're right next to the .csproj file)
dotnet list package
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add package Microsoft.EntityFrameworkCore.Proxies
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore
```

## Run and open
```
dotnet dev-certs https --trust          # optional, only needs to be done once to trust the self-signed certificate for https
dotnet build
dotnet run
code .
```

## Add data model and finish setting up EF
Install or update the tool (install is only needed once as it installs the tool globally)
```
dotnet tool install --global dotnet-ef
dotnet tool update --global dotnet-ef
```

### Add DB connection with connection string named SampleConnection.
Sample connection strings.  NOTE: The `\\` is for when this connection string is written inside the appsettings.json file, which must be in a JSON format.  To get a `\` char in JSON you must escape it as `\\`.  When you put this connection string into the dotnet user-secrets, to hide your username and password, you no longer need the escape sequence.

```
# Docker (password is in single quotes in case it has special characters, would still
# need to escape a ' as ''):
Data Source=localhost;Initial Catalog=AuctionHouse;User Id=sa;Password='Hello123#';

# LocalDB
Data Source=(localdb)\\MSSQLLocalDB;
```

### Create Model classes and the DBContext subclass
Reverse engineer C# model files from the DB schema found through the SampleConnection, placing them in the Models folder, and also create a DBContext subclass called SampleDbContext in the Models folder.  If older models exist already, the --force will overwrite them.

Please use the version with data annotations for this term.
```
dotnet ef dbcontext scaffold Name=SampleConnection Microsoft.EntityFrameworkCore.SqlServer --context SampleDbContext --context-dir Models --output-dir Models --verbose --force
# or if you want to use data annotations in your model classes rather than defining things in the context class
# try them one after another to see the difference in the generated classes
dotnet ef dbcontext scaffold Name=SampleConnection Microsoft.EntityFrameworkCore.SqlServer --context SampleDbContext --context-dir Models --output-dir Models --verbose --force  --data-annotations
```

### Enable Lazy loading of related properties
Open the DBContext subclass that was created in the last step.  In this example it is called `SampleDbContext.cs` and is located in the `Models` folder.  Find the `OnConfiguring` method and add the line to use lazy loading proxies:
```
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder
            .UseLazyLoadingProxies()        // <-- add this line
            .UseSqlServer("Name=SampleConnection");
    }
}
```
This version of modifying the DbContext subclass is problematic because the next time you regenerate your model classes it will be overwritten.  So it is better to make this change in `Program.cs` when your application is being initialized.  Here's where you can do that:
```
public static void Main(string[] args)
{
    // adds the following logging providers: Console, Debug, EventSource, EventLog (https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-6.0)
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllersWithViews();
    var connectionString = builder.Configuration.GetConnectionString("StreamingConnection");
    builder.Services.AddDbContext<StreamingDbContext>(options => options
                                .UseLazyLoadingProxies()    // Will use lazy loading, but not in LINQPad as it doesn't run Program.cs
                                .UseSqlServer(connectionString));
```


### Use LINQPad 8 to explore your data and build Linq queries
Download and install [LINQPad 8](https://www.linqpad.net/) (sorry, Windows only).

Make sure your project has built, then open LINQPad and click `Add connection`.  Select the radio button for "Use a typed data context from your own assembly" and choose "EntityFramework Core (3.x -> 7.x)".  Click Next.  Click Browse for the "Path to Custom Assembly" and go find your applications `.dll`  For this example it is in the bin folder of your applications source at `bin\Debug\net6.0\Sample.dll`.  After the dialog finds your DbContext class and populates the second text field, choose "Via a constructor that accepts a DbContextOptions<>" from the "How should LINQPad instantiate your DbContext?".  Click Test and hopefully it shows you that it can connect to the database through your applications code.

This process will be demonstrated in class.

### Scaffold CRUD operations in a controller with views
To see an example of CRUD operations for an entity we can scaffold a sample controller with associated views that perform each of the CRUD operations.  This first example does so in a traditional page load model with a normal MVC controller.

First install (or update) the code generator tool:
```
dotnet tool install -g dotnet-aspnet-codegenerator
dotnet tool update -g dotnet-aspnet-codegenerator
```

Then scaffold a controller with views for a particular model
```
dotnet-aspnet-codegenerator controller -name ItemController -m Show -dc AuctionHouseDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
```

Here is the documentation for this tool: [dotnet-aspnet-codegenerator](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/tools/dotnet-aspnet-codegenerator?view=aspnetcore-6.0)

Note, this uses the DbContext directly and so is only a guide or starting point for us since we are using Repositories.

For a WebAPI REST interface for CRUD operations we can change this slighly to generate a WebAPI controller with NO views:
```
dotnet-aspnet-codegenerator controller -name BuyerController -async -api -m Buyer -dc AuctionHouseDbContext -outDir Controllers
```
See [Tutorial: Create a web API with controllers](https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-6.0&tabs=visual-studio-code) for more.

Watch out for cycles though if you try to do this directly with your database models.  
