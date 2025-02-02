//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Identity.Web;

//namespace CleanSharpArchitecture.Extensions
//{
//    public static class WebSetupExtensions
//    {
//        // 1) Registrar serviços relacionados à Web
//        public static IServiceCollection AddWebSetup(this IServiceCollection services)
//        {
//            // Exemplos: Authentication, Controllers, Swagger

//            // Exemplo de configuração de Auth (JWT + Microsoft Identity)
//            services
//                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//                .AddMicrosoftIdentityWebApi(options => { /* ... */ },
//                                            configure => { /* ... */ });

//            services.AddControllers();

//            // Swagger
//            services.AddEndpointsApiExplorer();
//            services.AddSwaggerGen();

//            // Qualquer outra configuração “services.XXX” que estava no antigo Program.cs

//            return services;
//        }

//        // 2) Configurar middlewares (pipeline) do app
//        public static IApplicationBuilder UseWebSetup(this IApplicationBuilder app)
//        {
//            var env = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

//            if (env.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();
//            app.UseAuthentication();
//            app.UseAuthorization();

//            // Rotas
//            app.UseRouting();
//            app.UseEndpoints(endpoints =>
//            {
//                endpoints.MapControllers();
//            });

//            // Qualquer outro middleware que estivesse no antigo Program.cs

//            return app;
//        }
//    }
//}
