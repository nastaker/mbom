using System.Data.Entity;

namespace Repository
{
    class BaseDbInitializer : CreateDatabaseIfNotExists<BaseDbContext>
    {
        protected override void Seed(BaseDbContext context)
        {
        }
    }
}
