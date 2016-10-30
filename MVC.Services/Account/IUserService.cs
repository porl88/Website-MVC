namespace MVC.Services.Account
{
    using Microsoft.AspNet.Identity;
    using Transfer;

    public interface IUserService
    {
        CreateUserResponse CreateUser(CreateUserRequest request);

        //GetUserResponse GetUser(GetUserRequest request); //- userId, userName, email

        GetUsersResponse GetUsers(GetUsersRequest request);

        // EditUserResponse UpdateUser(EditUserRequest request);

        // DeleteUserResponse DeleteUser(DeleteUserRequest request);

        // SuspendUserResponse SuspendUser(SuspendUserRequest request);

        // UnlockUserResponse UnlockUser(UnlockUserRequest request);

        // GetAccountActivationToken???
    }
}
