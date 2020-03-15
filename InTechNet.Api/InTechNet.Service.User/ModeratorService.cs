﻿using System.Linq;
using InTechNet.Common.Utils.Authentication;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Authentication;
using InTechNet.Service.User.Helper;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User
{
    /// <inheritdoc cref="IModeratorService" />
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

        /// <inheritdoc cref="IModeratorService.AuthenticateModerator" />
        public ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData)
        {
            // Unwrap the provided connection data
            var (login, password) = (authenticationData.Login, authenticationData.Password);

            // Retrieve the user associated with this login
            var moderator = _context.Moderators
                .FirstOrDefault(_ =>
                    _.ModeratorNickname == login
                    || _.ModeratorEmail == login);

            if (moderator == null) throw new UnknownUserException();

            // Hash the provided raw password with the associated salt
            var hashedPassword = password.HashedWith(moderator.ModeratorSalt);

            // Assert that the provided password matches the stored one
            if (hashedPassword != moderator.ModeratorPassword) throw new InvalidCredentialsException();

            // Return the DTO associated to the moderator without its password
            return new ModeratorDto
            {
                Id = moderator.IdModerator,
                Email = moderator.ModeratorEmail,
                Nickname = moderator.ModeratorNickname,
                Passowrd = string.Empty
            };
        }
    }
}
