using ATM.Web.ViewModels;

namespace ATM.Web.Repository
{
    public interface IUserRepository
    {
        RegistrationSuccessViewModel CreateUser(RegisterViewModel registerViewModel);
    }
}
