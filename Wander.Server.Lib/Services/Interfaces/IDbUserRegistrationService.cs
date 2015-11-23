using Wander.Server.Model;

namespace Wander.Server.Services
{
    public interface IUserRegistrationService
    {
        /// <summary>
        /// Checks whether the UserModel fields matches the condition for the form validation
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the UserModel is valid, otherwise returns false</returns>
        bool CheckRegisterForm(UserModel user);

        /// <summary>
        /// Check if the password and login from the UserModel correspond to a User in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the login and password correspond to a User, otherwise returns false</returns>
        bool CheckLogin(UserModel user);

        /// <summary>
        /// Check if the provided UserModel's login already exists in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the UserModel's login already exists, otherwise return false</returns>
        bool CheckLoginAlreadyExists(UserModel user);

        /// <summary>
        /// Check if the provided UserModel's email already exists in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns true if the UserModel's email already exists, otherwise return false</returns>
        bool CheckEmailAlreadyExists(UserModel user);

        /// <summary>
        /// Change the Connected state of the provided UserModel to 1
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Returns the UserId of the user if it exists, otherwise returns -1</returns>
        int Connect(UserModel user);

        /// <summary>
        /// Register the UserModel in the database
        /// </summary>
        /// <param name="user"></param>
        void Register(UserModel user);

        /// <summary>
        /// Change the Connected state of the provided UserModel to 0
        /// </summary>
        /// <param name="user"></param>
        void LogOut(UserModel user);

        /// <summary>
        /// Change the Connected state of the provided ServerPlayerModel to 0
        /// </summary>
        /// <param name="user"></param>
        void LogOut(ServerPlayerModel user);
    }
}