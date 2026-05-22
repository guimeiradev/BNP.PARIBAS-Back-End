using Asp.Versioning.ApiExplorer;
using Bnp.Paribas.API.Middlewares;
using Microsoft.OpenApi;

namespace Bnp.Paribas.API.Extensions;

public static class ApiExtension
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "Desafio Bnp Paribas API",
                    Version = description.ApiVersion.ToString()
                });
            }
        });
        services.AddControllers();
        return services;
    }

    public static WebApplication UseApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
            });
        }

        app.UseLogging();
        app.UseCorsPolicy();
        app.UseHttpsRedirection();
        app.MapControllers();
        return app;
    }
}