﻿using InTechNet.Common.Dto.User;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Security;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Entity;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Registration;
using InTechNet.Service.Hub.Interfaces;
using InTechNet.Service.User.Helper;
using InTechNet.Service.User.Interfaces;
using System.Collections.Generic;
using System.Linq;

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
        /// TODO
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="hubService">TODO</param>
        public ModeratorService(InTechNetContext context, IHubService hubService)
        {
            _context = context;
            _hubService = hubService;
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
                Password = string.Empty
            };
        }

        /// <inheritdoc cref="IModeratorService.GetModerator" />
        public ModeratorDto GetModerator(int moderatorId)
        {
            var moderator = _context.Moderators
                .FirstOrDefault(_ => _.IdModerator == moderatorId) 
                            ?? throw new UnknownUserException();

            return new ModeratorDto
            {
                Password = string.Empty,
                Nickname = moderator.ModeratorNickname,
                Email = moderator.ModeratorNickname,
                Id = moderatorId,
                Hubs = _hubService.GetModeratorHubs(moderator.IdModerator)
            };
        }

        /// <inheritdoc cref="IModeratorService.RegisterModerator" />
        public void RegisterModerator(ModeratorDto newModeratorData)
        {
            // Assert that its nickname or email is unique in InTechNet database
            var isDuplicateTracked = _context.Moderators.Any(_ =>
                _.ModeratorNickname == newModeratorData.Nickname
                || _.ModeratorEmail == newModeratorData.Email);

            if (isDuplicateTracked)
            {
                throw new DuplicateIdentifierException();
            }

            // Generate a random salt for this moderator
            var salt = InTechNetSecurity.GetSalt();

            // Salting the password
            var saltedPassword = newModeratorData.Password.HashedWith(salt);

            // Record the new moderator
            _context.Moderators.Add(new Moderator
            {
                Hubs = new List<DataAccessLayer.Entity.Hub>(),
                ModeratorEmail = newModeratorData.Email,
                ModeratorNickname = newModeratorData.Nickname,
                ModeratorPassword = saltedPassword,
                ModeratorSalt = salt
            });

            _context.SaveChanges();
        }
    }
}
