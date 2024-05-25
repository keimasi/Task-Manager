using TaskManager.Models;
using TaskManager.Models.Dto;

namespace TaskManager.Services;

public class AuthService
{
    private readonly DataBaseContext _context;

    public AuthService(DataBaseContext context)
    {
        _context = context;
    }

    public bool Authenticate(LoginDto command)
    {
        var user = _context.Users.SingleOrDefault(x => x.UserName == command.Username);

        if (user == null)
            return false;

        return user.Password == command.Password;
    }
}