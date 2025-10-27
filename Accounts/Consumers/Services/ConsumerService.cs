using RentMaster.Accounts.Models;
using RentMaster.Accounts.Repositories;
using RentMaster.Accounts.Validator;
using RentMaster.Core.Exceptions;
using RentMaster.Core.Services;

namespace RentMaster.Accounts.Services
{
    public class ConsumerService : BaseService<Consumer>
    {
        private readonly ConsumerValidator _validator;

        public ConsumerService(ConsumerRepository repository, ConsumerValidator validator)
            : base(repository)
        {
            _validator = validator;
        }

        public override async Task<Consumer> CreateAsync(Consumer model)
        {
            var isEmailValid = await _validator.ValidateGmailAsync(model.Gmail);
            if (!isEmailValid)
                throw new ConflictException("The provided email address is already in use. Please use a different email or try signing in.");
            
            model.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            return await base.CreateAsync(model);
        }

        public override async Task UpdateAsync(Consumer model)
        {
            var isEmailValid = await _validator.ValidateGmailAsync(model.Gmail, model.Uid);
            if (!isEmailValid)
                throw new ConflictException("The provided email address is already in use by another account. Please use a different email address.");

            await base.UpdateAsync(model);
        }
    }
}