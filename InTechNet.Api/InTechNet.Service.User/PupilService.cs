using System.Linq;
using InTechNet.Common.Utils.Authentication;
using InTechNet.DataAccessLayer;
using InTechNet.Exception.Authentication;
using InTechNet.Service.User.Helper;
using InTechNet.Service.User.Interfaces;
using InTechNet.Service.User.Models;

namespace InTechNet.Service.User
{
    /// <inheritdoc cref="IPupilService"/>
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
        {
            _context = context;
        }

        /// <inheritdoc cref="IPupilService.AuthenticatePupil"/>
        public PupilDto AuthenticatePupil(AuthenticationDto authenticationData)
        {
            // Unwrap the provided connection data
            var (login, password) = (authenticationData.Login, authenticationData.Password);

            // Retrieve the user associated with this login
            var pupil = _context.Pupils
                .FirstOrDefault(_ =>
                    _.PupilEmail == login
                    || _.PupilNickname == login);

            if (pupil == null)
            {
                throw new UnknownUserException();
            }

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
                IdPupil = pupil.IdPupil,
                PupilEmail = pupil.PupilEmail,
                PupilNickname = pupil.PupilNickname,
            };
        }
    }
}
