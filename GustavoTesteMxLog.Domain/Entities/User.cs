namespace GustavoTesteMxLog.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = "";
    public string Email { get; private set; } = "";
    public string Password { get; private set; } = "";
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; private set; }

    public User() { }

    public User
    (
        string name,
        string email,
        string password
    ) 
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
    }

    private void SetName(string name) => Name = name;
    private void SetEmail(string email) => Email = email;
    private void SetPassword(string password) => Password = password;
    private void SetUpdatedAt() => UpdatedAt = DateTime.Now;

    public static User TryCreate
        (
            string name,
            string email,
            string password
        )
    {
        return new
            (
                name,
                email,
                password
            );
    }

    public static User TryUpdate
        (
            User user,
            string name,
            string email,
            string? password
        )
    {
        user.SetName(name);
        user.SetEmail(email);
        user.SetUpdatedAt();

        if (!string.IsNullOrWhiteSpace(password))
            user.SetPassword(password);

        return user;
    }
}
