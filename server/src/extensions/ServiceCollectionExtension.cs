using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using src.Database;
using src.features.shared.Interfaces;
using src.Utilities;

namespace src.extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterRepositories(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Types.Program.Assembly;
    
        List<Type> reposTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false } && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == Types.IBaseRepository))
            .ToList();
    
        foreach (Type repoType in reposTypes)
        {
            Type interfaceType = repoType.GetInterfaces().First();
            services.AddScoped(interfaceType, repoType);
        }
    }
    
    public static void RegisterEndpoints(this IServiceCollection services, Assembly? assembly = null)
    {
        assembly ??= Types.Program.Assembly;
        
        ServiceDescriptor[] endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(Types.IEndpoint))
            .Select(type => ServiceDescriptor.Transient(Types.IEndpoint, type))
            .ToArray();
        
        services.TryAddEnumerable(endpointServiceDescriptors);
    }

    public static IApplicationBuilder MapEndpoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.Configure(builder);
        }

        return app;
    }
    
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UsersDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"),
                optionsBuilder => optionsBuilder.MigrationsAssembly("src"));
        });
    }
}