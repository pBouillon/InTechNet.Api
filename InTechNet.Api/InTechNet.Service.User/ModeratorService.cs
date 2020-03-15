using InTechNet.Common.Utils.Authentication;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Authentication;
using InTechNet.Service.User.Helper;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using InTechNet.DataAccessLayer.Entity;

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
                Password = string.Empty
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
                // TODO: custom exception
                throw new System.Exception();
            }

            // Generate a random salt for this moderator
            // TODO: constant
            var saltBuffer = new byte[256];

            using var cryptoServiceProvider = new RNGCryptoServiceProvider();
            cryptoServiceProvider.GetNonZeroBytes(saltBuffer);

            var salt = Convert.ToBase64String(saltBuffer);

            // Salting the password
            var saltedPassword = newModeratorData.Password.HashedWith(salt);

            // Record the new moderator
            _context.Moderators.Add(new Moderator
            {
                Hubs = new List<Hub>(),
                ModeratorEmail = newModeratorData.Email,
                ModeratorNickname = newModeratorData.Nickname,
                ModeratorPassword = saltedPassword,
                ModeratorSalt = salt
            });

            _context.SaveChanges();
        }
    }
}
