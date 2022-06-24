namespace OrderSaga.DatabaseSchema
{
    public interface IDbMigrationRunner
    {
        void UpdateDatabase();
    }
}
