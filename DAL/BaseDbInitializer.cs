using System.Data.Entity;

namespace DAL
{
    class BaseDbInitializer : CreateDatabaseIfNotExists<BaseDbContext>
    {
        protected override void Seed(BaseDbContext context)
        {
        }
    }
}
