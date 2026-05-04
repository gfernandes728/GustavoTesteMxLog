using GustavoTesteMxLog.Application.Interfaces;
using System.Text.RegularExpressions;

namespace GustavoTesteMxLog.Application.Services;

public class ValidatorService : IValidatorService
{
    public bool IsEmailValid(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
    }
}
