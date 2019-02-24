namespace Pm.Core.Interfaces
{
    public interface IPasswordDerivationService
    {
        string CreateHash(string password);
        bool VerifyHash(string hash, string password);
    }
}