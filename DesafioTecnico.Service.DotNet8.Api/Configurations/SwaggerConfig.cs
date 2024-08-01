using Microsoft.OpenApi.Models;

namespace DesafioTecnico.Service.DotNet8.Api.Configurations
{
    public static class SwaggerConfig
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DesafioTecnico API", Version = "v1" });
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DesafioTecnico API v1"));
        }
    }
}
