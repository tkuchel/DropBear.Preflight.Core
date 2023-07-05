# Preflight Core

Preflight.Core is a .NET library that provides an easy-to-use preflight manager.

## Features

- Simple API for adding tasks to the preflight manager.
-
-
-
-

## Installation

You can add Preflight.Core to your project by using the NuGet package manager in Visual Studio, or by using the `dotnet add package` command in the .NET CLI:

```bash
dotnet add package DropBear.Preflight.Core
```

## Usage

First, register the Preflight.Core services in your `Startup.cs` file:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Other service configuration...

    services.AddPreflightCore();
}
```

Then, you can inject and use `IPreflightManager` in your classes:

```csharp
public class MyClass
{
    private readonly IPreflightManager _preflightManager;

    public MyClass(IPreflightManager preflightManager)
    {
        _preflightManager = preflightManager;
    }

    public async Task MyMethod()
    {
        var manager = new PreflightManager(config, logger);
        manager.AddTask(new ExampleTask(logger));
    }

    public class ExampleTask : PreflightTask
    {
        private ILogger<ExampleTask> logger;

        public ExampleTask(ILogger<ExampleTask> logger)
        {
            this.logger = logger;
        }

        public override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Log the start of the task
                logger.LogInformation("Starting ExampleTask");

                // Do some work...

                // Log the completion of the task
                logger.LogInformation("Completed ExampleTask");

                // Markthe task as completed
                MarkAsCompleted();
            }
            catch (Exception ex)
            {
                // Log the error
                logger.LogError(ex, "Error executing ExampleTask");

                throw;
            }
        }
    }
}

```

## Documentation

For more detailed documentation, please see the official documentation.

## Contributing

We welcome contributions! Please see our contributing guide for details.

## License

Preflight.Core is licensed under the MIT License.
