using System.Collections.Generic;
using System.Linq;
using InTechNet.Common.Dto.User.Pupil;
using InTechNet.Common.Utils.Authentication;
using InTechNet.Common.Utils.Security;
using InTechNet.DataAccessLayer;
using InTechNet.DataAccessLayer.Entities;
using InTechNet.Exception.Authentication;
using InTechNet.Exception.Registration;
using InTechNet.Services.User.Helpers;
using InTechNet.Services.User.Interfaces;

namespace InTechNet.Services.User
{
    /// <inheritdoc cref="IPupilService" />
    public class PupilService : IPupilService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public PupilService(InTechNetContext context)
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
            if (hashedPassword != pupil.PupilPassword) throw new InvalidCredentialsException();

            // Return the DTO associated to the moderator
            return new PupilDto
            {
                Id = pupil.IdPupil,
                Email = pupil.PupilEmail,
                Nickname = pupil.PupilNickname
            };
        }

        /// <inheritdoc cref="IPupilService.GetPupil" />
        public PupilDto GetPupil(int pupilId)
        {
            var pupil = _context.Pupils
                                .FirstOrDefault(_ => _.IdPupil == pupilId)
                            ?? throw new UnknownUserException();

            return new PupilDto
            {
                Nickname = pupil.PupilNickname,
                Email = pupil.PupilEmail,
                Id = pupilId
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
                Attendees = new List<Attendee>(),
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
            return _context.Pupils.Any(_ =>
                    _.PupilEmail == email);
        }

        /// <inheritdoc cref="IPupilService.IsNicknameAlreadyInUse" />
        public bool IsNicknameAlreadyInUse(string nickname)
        {
            return _context.Pupils.Any(_ =>
                    _.PupilNickname == nickname);
        }
    }
}
