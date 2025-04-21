using Domain.Customers;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Customers.Delete
{
    internal sealed class DeleteCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCustomerCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken)
        {
            if (await _customerRepository.GetByIdAsync(new CustomerId(command.Id), cancellationToken) is not Customer customer)
                return Error.NotFound("Customer.NotFound", "The Customer with the provide Id was not found.");
            
            await _customerRepository.Delete(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
