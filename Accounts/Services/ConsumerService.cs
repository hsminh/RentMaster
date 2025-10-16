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
        public async Task<IEnumerable<consumer>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<consumer?> GetByIdAsync(Guid id)
        {
            return await _repository.FindByIdAsync(id);
        }

        public async Task<consumer?> CreateAsync(consumer model)
        {
            var isValid = await _validator.ValidateGmailAsync(model.Gmail);
            if (!isValid)
                return null;

            return await _repository.CreateAsync(model);
        }


        public async Task UpdateAsync(Guid id, consumer model)
        {
            var isValid = await _validator.ValidateGmailAsync(model.Gmail, model.Uid);
            if (!isValid)
                throw new Exception("Gmail already exists for another user.");
            if (id != model.Uid)
                throw new ArgumentException("Mismatched consumer ID");

            await _repository.UpdateAsync(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            var consumer = await _repository.FindByIdAsync(id);
            if (consumer == null)
                throw new KeyNotFoundException("Consumer not found");

            await _repository.DeleteAsync(consumer);
        }
    }
}