using Microsoft.EntityFrameworkCore;
using RentMaster.Accounts.Models;
using RentMaster.Core.Repositories;
using RentMaster.Data;

namespace RentMaster.Accounts.Repositories
{
    public class ConsumerRepository: BaseRepository<Consumer>
    {
        public ConsumerRepository(AppDbContext context) : base(context)
        {
        }
    }
}