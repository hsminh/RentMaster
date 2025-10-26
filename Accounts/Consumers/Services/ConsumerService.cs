using RentMaster.Accounts.Models;
using RentMaster.Accounts.Repositories;
using RentMaster.Accounts.Validator;

namespace RentMaster.Accounts.Services
{
    public class ConsumerService
    {
        private readonly ConsumerRepository _repository;
        private readonly ConsumerValidator _validator;

        public ConsumerService(ConsumerRepository repository, ConsumerValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<IEnumerable<Consumer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Consumer?> GetByIdAsync(Guid id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public async Task<Consumer?> CreateAsync(Consumer model)
        {
            var isValid = await _validator.ValidateGmailAsync(model.Gmail);
            if (!isValid)
                return null;
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            return await _repository.CreateAsync(model);
        }


        public async Task UpdateAsync(Guid id, Consumer model)
        {
            var isValid = await _validator.ValidateGmailAsync(model.Gmail, model.Uid);
            if (!isValid)
                throw new Exception("Gmail already exists for another user.");
            if (id != model.Uid)
                throw new ArgumentException("Mismatched Consumer ID");

            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            var Consumer = await _repository.FindByIdAsync(id);
            if (Consumer == null)
                throw new KeyNotFoundException("Consumer not found");

            await _repository.DeleteAsync(Consumer);
        }
    }
}