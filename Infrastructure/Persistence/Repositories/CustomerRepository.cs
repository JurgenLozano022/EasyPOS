using Domain.Customers;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class CustomerRepository(ApplicationDbContext context) : ICustomerRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task Add(Customer customer) => await _context.Customers.AddAsync(customer);

        public async Task Delete(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<bool> ExistsAsync(CustomerId id) => await _context.Customers.AnyAsync(customer => customer.Id == id);

        public async Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Customers.ToListAsync();

        public Task<Customer?> GetByIdAsync(CustomerId id, CancellationToken cancellationToken = default) =>
            _context.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task Update(Customer customer)
        {
            _context.Customers.Update(customer);
        }
    }
}
