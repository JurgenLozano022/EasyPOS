﻿using Domain.Customers;
using Domain.Primitives;
using Domain.ValueObjects;
using ErrorOr;
using MediatR;

namespace Application.Customers.Update
{
    internal sealed class UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCustomerCommand, ErrorOr<Unit>>
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ErrorOr<Unit>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
        {
            if (!await _customerRepository.ExistsAsync(new CustomerId(command.Id)))
                return Error.NotFound("Customer.NotFound", "The customer with the provide Id was not found.");

            if (PhoneNumber.Create(command.PhoneNumber) is not PhoneNumber phoneNumber)
                return Error.Validation("Customer.PhoneNumber", "Phone number has not valid format.");

            if (Address.Create(command.Country, command.Line1, command.Line2, command.City, command.State, command.ZipCode) is not Address address)
                return Error.Validation("Customer.Address", "Address is not valid.");

            Customer customer = Customer.UpdateCustomer(
                command.Id,
                command.Name,
                command.LastName,
                command.Email,
                phoneNumber,
                address,
                command.Active
            );

            await _customerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
