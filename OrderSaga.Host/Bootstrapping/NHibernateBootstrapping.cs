using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;

namespace OrderSaga.Host.Bootstrapping
{
    public static class NHibernateBootstrapping
    {
        public static IServiceCollection ConfigurateNHibernate(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddSingleton(ConfigurateNHibernateFactory(configuration));

            return services;
        }

        private static ISessionFactory ConfigurateNHibernateFactory(IConfiguration configuration)
        {
            var nHibernateConfig = new Configuration()
                .DataBaseIntegration(c =>
                {
                    c.Dialect<MsSql2012Dialect>();
                    c.ConnectionString = configuration["ConnectionStrings:OrderSagaDB"];
                    c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
                    c.SchemaAction = SchemaAutoAction.Validate;
                    c.LogFormattedSql = true;
                    c.LogSqlInConsole = true;
                });

            var mapper = new ModelMapper();
            mapper.AddMappings(typeof(NHibernateBootstrapping).Assembly.ExportedTypes);
            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            nHibernateConfig.AddMapping(domainMapping);

            var sessionFactory = nHibernateConfig.BuildSessionFactory();

            return sessionFactory;
        }
    }
}
