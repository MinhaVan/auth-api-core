namespace Auth.Domain.Interfaces.Repository;

public interface IUserContext
{
    int UserId { get; }
    int Empresa { get; }
}