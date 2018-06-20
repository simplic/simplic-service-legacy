using Simplic.Authorization;
using Simplic.Group;
using Simplic.User;
using Simplic.UserSession;
using System.Linq;
using Unity;
using System;
using System.IO;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

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
            if (string.IsNullOrWhiteSpace(userName))
                throw new LoginFailedException(LoginFailedType.UserNameNotEntered);

            if (string.IsNullOrWhiteSpace(password))
                throw new LoginFailedException(LoginFailedType.PasswordNotEntered);

            var user = userService.GetByName(userName);

            var providerName = user.IsADUser ? "ActiveDirectoryCredentialProvider" : "DefaultCredentialProvider";
            var provider = container.Resolve<ICredentialProvider>(providerName);

            if (!provider.CheckCredentials(domain, userName, password, user.Password))
                throw new LoginFailedException(LoginFailedType.InvalidCredentials);

            return GenerateUserSession(user);
        }

        /// <summary>
        /// Generate user session by current user instance
        /// </summary>
        /// <param name="user">User instance</param>
        /// <returns>UserSession object if valid</returns>
        private Session GenerateUserSession(User.User user)
        {
            if (user == null)
                throw new LoginFailedException(LoginFailedType.UserNotFound);

            if (!user.IsActive)
                throw new LoginFailedException(LoginFailedType.UserNotActive);

            var userGroups = groupService.GetAllByUserId(user.Ident);
            var isSuperUser = userGroups.Any(x => x.GroupId == 0);
            var userGroupIdents = userGroups.Select(x => x.Ident).ToList();

            return new Session()
            {
                UserId = user.Ident,
                UserName = user.UserName,

                IsSuperUser = isSuperUser,
                UserAccessGroups = userGroupIdents,
                UserAccessGroupsBitMask = authorizationService.CreateBitMask(userGroupIdents),
                UserBitMask = authorizationService.CreateBitMask(user.Ident),
                IsADUser = user.IsADUser
            };
        }

        /// <summary>
        /// Activate autologin and write autologin file
        /// </summary>
        /// <param name="domain">Current domain</param>
        /// <param name="userName">Current user</param>
        /// <param name="password">Current password</param>
        public void SetAutologin(string domain, string userName, string password)
        {
            try
            {
                var path = GetAutologinPath();

                if (Login(domain, userName, password) != null)
                {
                    // Create autologin object
                    var hash = GenerateHash(userName, domain);
                    var loginObject = new AutologinModel() { Hash = hash, UserName = userName, Domain = domain };

                    // Ensure directory
                    IO.DirectoryHelper.CreateDirectoryIfNotExists(Path.GetDirectoryName(GetAutologinPath()));

                    File.WriteAllText(GetAutologinPath(), JsonConvert.SerializeObject(loginObject));
                }
            }
            catch
            {
                /* swallow */
            }
        }

        /// <summary>
        /// Remove autologin for the current windows user
        /// </summary>
        public void RemoveAutologin()
        {
            var path = GetAutologinPath();
            if (File.Exists(path))
                File.Delete(path);
        }

        /// <summary>
        /// Check whether autologin is existing and valid for the current user
        /// </summary>
        /// <returns>Simplic session if login was successfull else or null</returns>
        public Session TryAutologin()
        {
            Session session = null;

            var path = GetAutologinPath();
            if (File.Exists(path))
            {
                var obj = JsonConvert.DeserializeObject<AutologinModel>(File.ReadAllText(path));
                if (GenerateHash(obj.UserName, obj.Domain) == obj.Hash)
                {
                    var user = userService.GetByName(obj.UserName);
                    return GenerateUserSession(user);
                }
            }

            return session;
        }

        /// <summary>
        /// Generate unique hash
        /// </summary>
        /// <returns>Unique hash for the given user</returns>
        private string GenerateHash(string userName, string domain)
        {
            return Security.Cryptography.CryptographyHelper.HashSHA256(NetworkInterface
                   .GetAllNetworkInterfaces()
                   .Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                   .Select(nic => nic.GetPhysicalAddress().ToString())
                   .FirstOrDefault() + $"{Environment.UserDomainName}_{Environment.UserName}_{userName}_{domain}");
        }

        /// <summary>
        /// Get autologin file path
        /// </summary>
        /// <returns>Autologin file path</returns>
        private string GetAutologinPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Simplic Studio\\Login.json";
        }
    }
}