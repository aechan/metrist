# metrist
Automatically record your Medium posts into the Bitcoin blockchain using po.et's Frost API.

## Development setup
Make sure .NET Core command line tools are installed.

In root of project run `dotnet restore` to restore Nuget packages.

In the `public` folder run `npm install` to restore npm dependencies.

In the root of the project, run `dotnet run <port>` where port number is optional and defaults to 8080 if not provided.

To run on port 80 or other protected port, the command must be run with elevated permissions e.g. `sudo dotnet run 80`

## Check it out: http://metrist.org/
