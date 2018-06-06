# IoC.Discovery
MEF-based discovery library for **ASPNet Core** / `Microsoft.Extensions.DependencyInjection`.

## Introduction

The problem with all DI systems is that configuration generally is pulled in two polar-opposite directions. Either your container is configured by convention (with a few rare exception cases), or it's configured directly and declaratively with *masses* of registrations.

The latter configuration practice has the benefit of not having any side-effects as you bring in third party libraries - you'll not get random `Controller` classes added to your system, for example. And this is why it's my preference.

BUT, declaring every dependency in your container can lead to the dreaded "God Configuration Method" - where one method has hundreds of lines of container registration. This bad - it couples your application tightly to all dependencies at all levels, and makes it hard to maintain.

For the last few years, I've been using a discovery system for Unity that moves the registrations for dependencies away from the "God Configuration Method" and into individual "Bootstrappers" that live alongside the implementation of the service that they register.

This "magic" discovery pattern works well - reducing the "God Configuration Method" to a single line - and allowing (for example) client libraries to define the "right" lifecycles themselves, not relying on a programmer getting it right each time they use the client.

This library is a port of that Unity MEF Discovery pattern to the DI framework used in **ASPNet Core** - `Microsoft.Extensions.DependencyInjection`.

## Usage

Using the discovery library is simple. First install the NuGet package:

```powershell
install-package CheviotConsulting.DependencyInjection.Discovery
```

Next, where previously you would add registrations into the `Startup.ConfigureServices(...)` method, you write a `IServiceDiscoveryBootstrapper` implementation

```csharp
public sealed class WebAppServiceBootstrapper : IServiceDiscoveryBootstrapper
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IWebAppService, MyWebAppService>();
        }
    }
```

You can have as many of these as you like, in as many referenced projects as you like - which allows you to put a service registration alongside a service or service client itself.

Then in the `Startup.ConfigureServices(...)` method, you add the following single line:

```csharp
using Microsoft.Extensions.DependencyInjection.Discovery;

public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        ...
        services.BootstrapByDiscovery();
    }
```

And that's it. The discovery framework will load any assemblies referenced by your application, find all `IServiceDiscoveryBootstrapper` instances and execute the `ConfigureServices(...)` method on each in turn - bootstrapping the `IServiceCollection` used by **ASPNet Core** / `Microsoft.Extensions.DependencyInjection`.

## Advanced Usage

### Bootstrapper Dependencies

Unlike the `Startup.ConfigureServices` method, you can't pass dependencies in the method parameters. However, by default the bootstrapper classes themselves are resolved using the `IServiceCollection` as it stands just before the call to `BootstrapByDiscovery()` is made. So you can use constructor dependency injection in your bootstrapper.

```csharp
public sealed class SampleServicesBootstrapper : IServiceDiscoveryBootstrapper
{
    private readonly IHttpContextAccessor contextAccessor;

    public SampleServicesBootstrapper(IHttpContextAccessor contextAccessor)
    {
        this.contextAccessor = contextAccessor;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<MyCustomService>(new MyCustomService(contextAccessor));
    }
}
```

### Manual Bootstrapping

If you don't want to rely on the "magic" discovery, you can always bootstrap each of your bootstrappers individually within your `Startup.ConfigureServices(...)` method.

```csharp
public class Startup
{
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        ...

        // Bootstrap services manually
        services.BootstrapFrom<SampleServicesBootstrapper>();
        services.BootstrapFrom(new WebAppServiceBootstrapper());
    }
}
```