//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using CleanSharpArchitecture.Application.Repositories.Interfaces;
//using CleanSharpArchitecture.Infrastructure.Repositories;

//namespace CleanSharpArchitecture.Application.Extensions
//{
//    public static class ApplicationRepositoriesExtensions
//    {
//        public static IServiceCollection AddRepositories(this IServiceCollection services)
//        {
//            // Registra repositórios
//            services.AddScoped<IUserRepository, UserRepository>(); // Exemplo de registro de repositório
//            // Adicione outros repositórios aqui
//            // services.AddScoped<IOtherRepository, OtherRepository>();

//            return services;
//        }
//    }
//}
