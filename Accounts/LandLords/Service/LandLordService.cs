using RentMaster.Accounts.Models;
using RentMaster.Accounts.Repositories;
using RentMaster.Accounts.Validator;

namespace RentMaster.Accounts.Services
{
    public class LandLordService
    {
        private readonly LandLordRepository _repository;
        // private readonly LandLordValidator _validator;

        public LandLordService(LandLordRepository repository)
        {
            _repository = repository;
            // _validator = validator;
        }
        public async Task<IEnumerable<LandLord>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<LandLord?> GetByIdAsync(Guid id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public async Task<LandLord?> CreateAsync(LandLord model)
        {
            // var isValid = await _validator.ValidateGmailAsync(model.Gmail);
            // if (!isValid)
            //     return null;
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            return await _repository.CreateAsync(model);
        }


        public async Task UpdateAsync(Guid id, LandLord model)
        {
            // var isValid = await _validator.ValidateGmailAsync(model.Gmail, model.Uid);
            // if (!isValid)
            //     throw new Exception("Gmail already exists for another user.");
            if (id != model.Uid)
                throw new ArgumentException("Mismatched LandLord ID");

            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            var LandLord = await _repository.FindByIdAsync(id);
            if (LandLord == null)
                throw new KeyNotFoundException("LandLord not found");

            await _repository.DeleteAsync(LandLord);
        }
    }
}