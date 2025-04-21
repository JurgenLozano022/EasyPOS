using Application.Customers.Common;
using Domain.Customers;
using ErrorOr;
using MediatR;

namespace Application.Customers.GetById
{
    internal sealed class GetCustomerByIdQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetCustomerByIdQuery, ErrorOr<CustomerResponse>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<ErrorOr<CustomerResponse>> Handle(GetCustomerByIdQuery request, CancellationToken cancellationToken)
        {
            if (await _customerRepository.GetByIdAsync(new CustomerId(request.Id)) is not Customer customer) return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found."); ;
            
            return new CustomerResponse(
                customer.Id.Value,
                customer.FullName,
                customer.Email,
                customer.PhoneNumber.Value,
                new AddressResponse(
                    customer.Address.Country,
                    customer.Address.Line1,
                    customer.Address.Line2,
                    customer.Address.City,
                    customer.Address.State,
                    customer.Address.ZipCode),
                customer.Active);
        }
    }
}
