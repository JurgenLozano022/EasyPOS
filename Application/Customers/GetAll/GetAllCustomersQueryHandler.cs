using Application.Customers.Common;
using Domain.Customers;
using ErrorOr;
using MediatR;

namespace Application.Customers.GetAll
{
    internal sealed class GetAllCustomersQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomersQuery, ErrorOr<List<CustomerResponse>>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<ErrorOr<List<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.GetAllAsync(cancellationToken);

            return customers
                .Select(c => new CustomerResponse(
                    c.Id.Value,
                    c.FullName,
                    c.Email,
                    c.PhoneNumber.Value,
                    new AddressResponse(
                        c.Address.Country,
                        c.Address.Line1,
                        c.Address.Line2,
                        c.Address.City,
                        c.Address.State,
                        c.Address.ZipCode),
                    c.Active))
                .ToList();
        }
    }
}
