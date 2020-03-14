using System;
using System.Linq;
using InTechNet.Common.Utils.Authentication;
using InTechNet.DataAccessLayer;
using InTechNet.Service.User.Helper;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User
{
    /// <inheritdoc cref="IModeratorService"/>
    public class ModeratorService : IModeratorService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public ModeratorService(InTechNetContext context)
        {
            _context = context;
        }

        /// <inheritdoc cref="IModeratorService.AuthenticateModerator"/>
        public ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData)
        {
            var (login, password) = (authenticationData.Login, authenticationData.Password);

            var moderator = _context.Moderators
                .FirstOrDefault(_ =>
                    _.ModeratorEmail == login 
                    || _.ModeratorPassword == login);

            if (moderator == null)
            {
                // TODO: user not found exc
                throw new Exception();
            }

            var hashedPassword = password.HashedWith(moderator.ModeratorSalt);

            if (moderator.ModeratorPassword != hashedPassword)
            {
                // TODO: invalid credentials
                throw new Exception();
            }

            return new ModeratorDto
            {
                IdModerator = moderator.IdModerator,
                ModeratorEmail = moderator.ModeratorEmail,
                ModeratorNickname = moderator.ModeratorNickname,
            };
        }
    }
}
