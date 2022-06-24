using Microsoft.Extensions.DependencyInjection;
using OrderSaga.DatabaseSchema.Bootstrapping;

namespace OrderSaga.DatabaseSchema
{
    public class Program
    {
        public static void Main()
        {
            var serviceProvider = new ServiceCollection()
                .BootstrapMigration()
                .BuildServiceProvider();

            var runner = serviceProvider.GetRequiredService<IDbMigrationRunner>();

            runner.UpdateDatabase();
        }
    }
}
