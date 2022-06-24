using FluentMigrator.Runner;

namespace OrderSaga.DatabaseSchema
{
    public class DbMigrationRunner : IDbMigrationRunner
    {
        private readonly IMigrationRunner _migrationRunner;

        public DbMigrationRunner(IMigrationRunner migrationRunner)
        {
            _migrationRunner = migrationRunner;
        }

        public void UpdateDatabase()
        {
            _migrationRunner.MigrateUp();
        }
    }
}
