using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjektSwagger.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;


namespace ProjektSwagger {
    public class Program {
        public static void Main(string[] args) {
            // Program sam za uzytkownika przygotuje baze danych. Wystarczy kliknac "run".
            Console.WriteLine("* Initializing DB\t*");
            EmployeeContextInitializer init = new EmployeeContextInitializer();
            EmployeeContext _context = new EmployeeContext();
            init.InitializeDatabase(_context);

            Console.WriteLine("* Opening Swagger\t*");
            // Otwiera Swagger - w nim mo¿na testowaæ dzia³anie aplikacji.
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
