using Simplic.Authorization;
using Simplic.Group;
using Simplic.User;
using Simplic.UserSession;
using System.Linq;
using Unity;

namespace Simplic.Authentication.Service
{
    /// <summary>
    /// Authentication service handles logging a user in and holding the current session data
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService userService;
        private readonly IGroupService groupService;
        private readonly IUnityContainer container;
        private readonly IAuthorizationService authorizationService;

        /// <summary>
        /// Create new authentication service
        /// </summary>
        /// <param name="container">Unity container</param>
        /// <param name="userService">User service</param>
        /// <param name="authorizationService">Authorization service</param>
        /// <param name="groupService">Group service</param>
        public AuthenticationService(IUnityContainer container, IAuthorizationService authorizationService, IUserService userService, IGroupService groupService)
        {
            this.userService = userService;
            this.container = container;
            this.authorizationService = authorizationService;
            this.groupService = groupService;
        }
        
        /// <summary>
        /// Authenticate a user and create a use session
        /// </summary>
        /// <param name="domain">Domain</param>
        /// <param name="userName">User name</param>
        /// <param name="password">Password</param>
        /// <returns>A user session if the user could be logged in, else an exception will be thrown</returns>
        public Session Login(string domain, string userName, string password)
        {
            Session session = null;

            if (string.IsNullOrWhiteSpace(userName))
                throw new LoginFailedException(LoginFailedType.UserNameNotEntered);

            if (string.IsNullOrWhiteSpace(password))
                throw new LoginFailedException(LoginFailedType.PasswordNotEntered);

            var user = userService.GetByName(userName);

            if (user == null)
                throw new LoginFailedException(LoginFailedType.UserNotFound);

            if (!user.IsActive)
                throw new LoginFailedException(LoginFailedType.UserNotActive);

            var providerName = user.IsADUser ? "ActiveDirectoryCredentialProvider" : "DefaultCredentialProvider";
            var provider = container.Resolve<ICredentialProvider>(providerName);

            if (!provider.CheckCredentials(domain, userName, password, user.Password))
                throw new LoginFailedException(LoginFailedType.InvalidCredentials);

            var userGroups = groupService.GetAllByUserId(user.Ident);
            var isSuperUser = userGroups.Any(x => x.GroupId == 0);
            var userGroupIdents = userGroups.Select(x => x.Ident).ToList();

            session = new Session()
            {
                UserId = user.Ident,
                UserName = user.UserName,
                
                IsSuperUser = isSuperUser,
                UserAccessGroups = userGroupIdents,
                UserAccessGroupsBitMask = authorizationService.CreateBitMask(userGroupIdents),
                UserBitMask = authorizationService.CreateBitMask(user.Ident),
                IsADUser = user.IsADUser
            };

            return session;
        }
    }
}