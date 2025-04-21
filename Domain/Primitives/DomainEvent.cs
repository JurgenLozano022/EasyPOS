using MediatR;

namespace Domain.Primitives
{
    public record DomainEvent(Guid Id) : INotification
    {
        public DateTime OccurredOn { get; } = DateTime.UtcNow;
    }
}
