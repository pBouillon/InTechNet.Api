using InTechNet.DataAccessLayer;
using InTechNet.Service.User.Interfaces;

namespace InTechNet.Service.User
{
    public class PupilService : IPupilService
    {
        private readonly InTechNetContext _context;

        public PupilService(InTechNetContext context)
        {
            _context = context;
        }
    }
}
