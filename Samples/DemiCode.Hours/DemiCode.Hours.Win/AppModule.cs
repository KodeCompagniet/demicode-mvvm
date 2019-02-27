using Autofac;
using System.Configuration;
using System.Data.SqlClient;
using DemiCode.Hours.Shared.Services;
using DemiCode.Hours.Sql.Services;

namespace DemiCode.Hours.Win
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register services
            RegisterServices(builder);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["DemiCode.Hours.Sql.Properties.Settings.HoursConnectionString"].ConnectionString;
            //var connection = new SqlConnection(connectionString);
            //var hoursDataService = new SqlHoursDataService(connection);

            //builder.Register(hoursDataService).As<IHoursDataService>();

            builder.RegisterInstance<SqlConnectionFactory>(() =>
            {
                var connectionString = ConfigurationManager.ConnectionStrings["DemiCode.Hours.Sql.Properties.Settings.HoursConnectionString"].ConnectionString;
                return new SqlConnection(connectionString);
            });

            builder.RegisterType<SqlHoursDataService>().SingleInstance().As<IHoursDataService>();
        }
    }
}
