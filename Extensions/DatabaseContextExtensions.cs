using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ltbdb.Core.Data;
using ltbdb.Provider;

namespace ltbdb.Extensions
{
	public static class DatabaseContextExtensions
	{
		public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString, DatabaseProvider provider)
		{
			services.AddDbContext<DataContext>(options =>
			{
				switch (provider)
				{
					case DatabaseProvider.MySql:
						options.UseMySql(connectionString, mySqlOptions => { });
						break;

					case DatabaseProvider.PgSql:
						options.UseNpgsql(connectionString, npgSqlOptions => { });
						break;
				}
#if DEBUG
				options.EnableSensitiveDataLogging(true);
#endif
			},
			ServiceLifetime.Scoped);

			return services;
		}
	}
}