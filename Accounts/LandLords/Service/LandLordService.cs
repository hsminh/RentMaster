using RentMaster.Accounts.Models;
using RentMaster.Accounts.Repositories;
using RentMaster.Core.Services;

namespace RentMaster.Accounts.Services
{
    public class LandLordService : BaseService<LandLord>
    {
        public LandLordService(LandLordRepository repository)
            : base(repository)
        {
        }

        public override async Task<LandLord> CreateAsync(LandLord model)
        {
            if (!string.IsNullOrEmpty(model.Password))
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            return await base.CreateAsync(model);
        }

        public override async Task UpdateAsync(LandLord model)
        {
            if (!string.IsNullOrEmpty(model.Password))
            {
                model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            await base.UpdateAsync(model);
        }
    }
}