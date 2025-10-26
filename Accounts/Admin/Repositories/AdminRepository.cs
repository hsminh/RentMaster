using RentMaster.Core.Repositories;
using RentMaster.Data;

namespace RentMaster.Accounts.Repositories
{
    public class AdminRepository: BaseRepository<Models.Admin>
    {
        public AdminRepository(AppDbContext context) : base(context)
        {
        }
    }
}