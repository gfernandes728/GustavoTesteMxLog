namespace GustavoTesteMxLog.Domain.Responses;

public class UserResponse
(
    Guid id,
    string name,
    string email,
    DateTime createdAt,
    DateTime? updatedAt = null
)
{
    public Guid Id { get; private set; } = id;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime? UpdatedAt { get; private set; } = updatedAt;
}
