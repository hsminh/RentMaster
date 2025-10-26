using RentMaster.Accounts.Repositories;
using RentMaster.Accounts.Validator;

namespace RentMaster.Accounts.Services
{
    public class AdminService
    {
        private readonly AdminRepository _repository;
        private readonly AdminValidator _validator;

        public AdminService(AdminRepository repository, AdminValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<IEnumerable<Models.Admin>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Models.Admin?> GetByIdAsync(Guid id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public async Task<Models.Admin?> CreateAsync(Models.Admin model)
        {
            var isValid = await _validator.ValidateGmailAsync(model.Gmail);
            if (!isValid)
                return null;
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            return await _repository.CreateAsync(model);
        }


        public async Task UpdateAsync(Guid id, Models.Admin model)
        {
            var isValid = await _validator.ValidateGmailAsync(model.Gmail, model.Uid);
            if (!isValid)
                throw new Exception("Gmail already exists for another user.");
            if (id != model.Uid)
                throw new ArgumentException("Mismatched Admin ID");

            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            var Admin = await _repository.FindByIdAsync(id);
            if (Admin == null)
                throw new KeyNotFoundException("Admin not found");

            await _repository.DeleteAsync(Admin);
        }
    }
}