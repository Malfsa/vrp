using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace WpfApp2
{

        static class DbRegistrator
        {

            public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration Configuration) => services
                .AddDbContext<MyDbContext>(opt =>
                {
                    var type = Configuration["Type"];
                    switch (type)
                    {
                        case null: throw new InvalidOperationException("Не определён тип БД");
                        default: throw new InvalidOperationException($"Тип подключения {type} не поддерживается");

                        case "MSSQL":
                            opt.UseSqlServer(Configuration.GetConnectionString(type));
                            break;

                        case "POSTGRESQL":
                            opt.UseNpgsql(Configuration.GetConnectionString(type));
                            break;
                    }
                });

        }
    }



