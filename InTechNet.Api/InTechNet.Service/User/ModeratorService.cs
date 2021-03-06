﻿using InTechNet.Common.Dto.Subscription;
using InTechNet.Common.Dto.User.Moderator;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Security;
using InTechNet.Common.Utils.SubscriptionPlan;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Users;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Registration;
using InTechNet.Services.Hub.Interfaces;
using InTechNet.Services.User.Helpers;
using InTechNet.Services.User.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.Services.User
{
    /// <inheritdoc cref="IModeratorService" />
    public class ModeratorService : IModeratorService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IInTechNetContext _context;

        /// <summary>
        /// Hub service for hub related operations
        /// </summary>
        private readonly IHubService _hubService;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="hubService">Service for hub's operations</param>
        public ModeratorService(IInTechNetContext context, IHubService hubService)
            => (_context, _hubService) = (context, hubService);

        /// <inheritdoc cref="IModeratorService.AuthenticateModerator" />
        public ModeratorDto AuthenticateModerator(AuthenticationDto authenticationData)
        {
            // Unwrap the provided connection data
            var (login, password) = (authenticationData.Login, authenticationData.Password);

            // Retrieve the user associated with this login
            var moderator = _context.Moderators
                .Include(_ => _.ModeratorSubscriptionPlan)
                .Include(_ => _.Hubs)
                .FirstOrDefault(_ =>
                    _.ModeratorNickname == login
                    || _.ModeratorEmail == login);

            if (moderator == null) throw new UnknownUserException();

            // Hash the provided raw password with the associated salt
            var hashedPassword = password.HashedWith(moderator.ModeratorSalt);

            // Assert that the provided password matches the stored one
            if (hashedPassword != moderator.ModeratorPassword)
            {
                throw new InvalidCredentialsException();
            }

            var moderatorSubscriptionPlan = moderator.ModeratorSubscriptionPlan;

            SubscriptionPlanDto subscriptionPlanDtoForCurrentModerator = new SubscriptionPlanDto
            {
                IdSubscriptionPlan = moderatorSubscriptionPlan.Id,
                MaxHubPerModeratorAccount = moderatorSubscriptionPlan.MaxHubPerModeratorAccount,
                SubscriptionPlanName = moderatorSubscriptionPlan.SubscriptionPlanName,
                SubscriptionPlanPrice = moderatorSubscriptionPlan.SubscriptionPlanPrice,
                MaxAttendeesPerHub = moderatorSubscriptionPlan.MaxAttendeesPerHub,
                MaxModulePerHub = moderatorSubscriptionPlan.MaxModulePerHub,
            };

            // Return the DTO associated to the moderator without its password
            return new ModeratorDto
            {
                Id = moderator.Id,
                Email = moderator.ModeratorEmail,
                Nickname = moderator.ModeratorNickname,
                NumberOfHub = moderator.Hubs.Count(),
                SubscriptionPlanDto = subscriptionPlanDtoForCurrentModerator
            };
        }

        /// <inheritdoc cref="IModeratorService.DeleteModerator" />
        public void DeleteModerator(ModeratorDto moderatorDto)
        {
            var moderator = _context.Moderators.FirstOrDefault(_ =>
                    _.Id == moderatorDto.Id)
                ?? throw new UnknownUserException();

            _context.Moderators.Remove(moderator);
            _context.SaveChanges();
        }

        /// <inheritdoc cref="IModeratorService.GetModerator" />
        public ModeratorDto GetModerator(int moderatorId)
        {
            var moderator = _context.Moderators
                .Include(_ => _.ModeratorSubscriptionPlan)
                .Include(_ => _.Hubs)
                .FirstOrDefault(_ => _.Id == moderatorId) 
                            ?? throw new UnknownUserException();

            var moderatorSubscriptionPlan = moderator.ModeratorSubscriptionPlan;

            SubscriptionPlanDto subscriptionPlanDtoForCurrentModerator = new SubscriptionPlanDto
            {
                IdSubscriptionPlan = moderatorSubscriptionPlan.Id,
                MaxHubPerModeratorAccount = moderatorSubscriptionPlan.MaxHubPerModeratorAccount,
                SubscriptionPlanName = moderatorSubscriptionPlan.SubscriptionPlanName,
                SubscriptionPlanPrice = moderatorSubscriptionPlan.SubscriptionPlanPrice,
                MaxAttendeesPerHub = moderatorSubscriptionPlan.MaxAttendeesPerHub,
                MaxModulePerHub = moderatorSubscriptionPlan.MaxModulePerHub,
            };

            return new ModeratorDto
            {
                Nickname = moderator.ModeratorNickname,
                Email = moderator.ModeratorNickname,
                Id = moderatorId,
                NumberOfHub = moderator.Hubs.Count(),
                SubscriptionPlanDto = subscriptionPlanDtoForCurrentModerator
            };
        }

        /// <inheritdoc cref="IModeratorService.RegisterModerator" />
        public void RegisterModerator(ModeratorRegistrationDto newModeratorData)
        {
            // Assert that the provided password is safe enough
            var password = newModeratorData.Password;
            if (!PasswordHelper.IsStrongEnough(password))
            {
                throw new InvalidCredentialsException();
            }

            // Assert that its nickname or email is unique in InTechNet database
            if (IsEmailAlreadyInUse(newModeratorData.Email))
            {
                throw new DuplicatedEmailException();
            }

            if (IsNicknameAlreadyInUse(newModeratorData.Nickname))
            {
                throw new DuplicatedIdentifierException();
            }

            // Generate a random salt for this moderator
            var salt = InTechNetSecurity.GetSalt();

            // Salting the password
            var saltedPassword = newModeratorData.Password.HashedWith(salt);

            // Getting the free subscription
            var subscription = _context.SubscriptionPlans.First(_ =>
                _.SubscriptionPlanName == new FreeSubscriptionPlan().SubscriptionPlanName);

            // Record the new moderator
            _context.Moderators.Add(new Moderator
            {
                Hubs = new List<DataAccessLayer.Entities.Hubs.Hub>(),
                ModeratorEmail = newModeratorData.Email,
                ModeratorNickname = newModeratorData.Nickname,
                ModeratorPassword = saltedPassword,
                ModeratorSalt = salt,
                ModeratorSubscriptionPlan = subscription
            });

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IModeratorService.RegisterModerator" />
        public bool IsEmailAlreadyInUse(string email)
        {
            return _context.Moderators.Any(_ =>
                    _.ModeratorEmail == email);
        }

        /// <inheritdoc cref="IModeratorService.IsNicknameAlreadyInUse" />
        public bool IsNicknameAlreadyInUse(string nickname)
        {
            return _context.Moderators.Any(_ =>
                    _.ModeratorNickname == nickname);
        }
    }
}
