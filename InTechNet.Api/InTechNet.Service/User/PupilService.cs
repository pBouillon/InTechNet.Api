using InTechNet.Common.Dto.Hub;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Security;
using InTechNet.DataAccessLayer.Context;
using InTechNet.DataAccessLayer.Entities.Users;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Hub;
using InTechNet.Exception.Registration;
using InTechNet.Services.User.Helpers;
using InTechNet.Services.User.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace InTechNet.Services.User
{
    /// <inheritdoc cref="IPupilService" />
    public class PupilService : IPupilService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IInTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public PupilService(IInTechNetContext context)
            => _context = context;

        /// <inheritdoc cref="IPupilService.AuthenticatePupil" />
        public PupilDto AuthenticatePupil(AuthenticationDto authenticationData)
        {
            // Unwrap the provided connection data
            var (login, password) = (authenticationData.Login, authenticationData.Password);

            // Retrieve the user associated with this login
            var pupil = _context.Pupils
                .FirstOrDefault(_ =>
                    _.PupilEmail == login
                    || _.PupilNickname == login);

            if (pupil == null) throw new UnknownUserException();

            // Hash the provided raw password with the associated salt
            var hashedPassword = password.HashedWith(pupil.PupilSalt);

            // Assert that the provided password matches the stored one
            if (hashedPassword != pupil.PupilPassword)
            {
                throw new InvalidCredentialsException();
            }

            // Return the DTO associated to the moderator
            return new PupilDto
            {
                Id = pupil.Id,
                Email = pupil.PupilEmail,
                Nickname = pupil.PupilNickname
            };
        }

        /// <inheritdoc cref="IPupilService.DeletePupil" />
        public void DeletePupil(PupilDto pupilDto)
        {
            var pupil = _context.Pupils.FirstOrDefault(_ =>
                    _.Id == pupilDto.Id)
                ?? throw new UnknownUserException();

            _context.Pupils.Remove(pupil);
            _context.SaveChanges();
        }

        /// <inheritdoc cref="IPupilService.GetPupil" />
        public PupilDto GetPupil(int pupilId)
        {
            var pupil = _context.Pupils
                                .FirstOrDefault(_ => _.Id == pupilId)
                            ?? throw new UnknownUserException();

            return new PupilDto
            {
                Nickname = pupil.PupilNickname,
                Email = pupil.PupilEmail,
                Id = pupilId
            };
        }

        /// <inheritdoc cref="IPupilService.GetHubByLink"/>
        public PupilHubDto GetHubByLink(PupilDto pupilDto, string hubLink)
        {
            // Get the hub identified by the link
            var hub = _context.Hubs.Include(_ => _.Moderator)
                .Include(_ => _.Attendees)
                .FirstOrDefault(_ =>
                    _.HubLink == hubLink
                    && _.Attendees.Any(_ =>
                        _.Pupil.Id == pupilDto.Id))
                ?? throw new UnknownHubException();

            return new PupilHubDto
            {
                Id = hub.Id,
                Description = hub.HubDescription,
                ModeratorNickname = hub.Moderator.ModeratorNickname,
                Name = hub.HubName,
            };
        }

        /// <inheritdoc cref="IPupilService.RegisterPupil" />
        public void RegisterPupil(PupilRegistrationDto newPupilData)
        {
            // Assert that its nickname or email is unique in InTechNet database
            var isEmailDuplicated = _context.Pupils.Any(_ =>
                _.PupilEmail == newPupilData.Email);

            var isNicknameDuplicated = _context.Pupils.Any(_ =>
                _.PupilNickname == newPupilData.Nickname);

            if (isEmailDuplicated)
            {
                throw new DuplicatedEmailException();
            }

            if (isNicknameDuplicated)
            {
                throw new DuplicatedIdentifierException();
            }

            // Generate a random salt for this moderator
            var salt = InTechNetSecurity.GetSalt();

            // Salting the password
            var saltedPassword = newPupilData.Password.HashedWith(salt);

            // Record the new moderator
            _context.Pupils.Add(new Pupil
            {
                Attendees = new List<DataAccessLayer.Entities.Hubs.Attendee>(),
                PupilEmail = newPupilData.Email,
                PupilNickname = newPupilData.Nickname,
                PupilPassword = saltedPassword,
                PupilSalt = salt,
            });

            _context.SaveChanges();
        }

        /// <inheritdoc cref="IPupilService.IsEmailAlreadyInUse" />
        public bool IsEmailAlreadyInUse(string email)
        {
            return _context.Pupils.Any(_ 
                =>  _.PupilEmail == email);
        }

        /// <inheritdoc cref="IPupilService.IsNicknameAlreadyInUse" />
        public bool IsNicknameAlreadyInUse(string nickname)
        {
            return _context.Pupils.Any(_ 
                => _.PupilNickname == nickname);
        }
    }
}
