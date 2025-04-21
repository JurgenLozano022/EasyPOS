namespace Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default);
        Task Add(Customer customer);
        Task Update(Customer customer);
        Task Delete(Customer customer);
        Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(CustomerId id);
    }
}
