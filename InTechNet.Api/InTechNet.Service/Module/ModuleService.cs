using InTechNet.DataAccessLayer;
using InTechNet.Services.Module.Interfaces;

namespace InTechNet.Services.Module
{
    /// <inheritdoc cref="IModuleService"/>
    public class ModuleService : IModuleService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly InTechNetContext _context;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="context">Database context</param>
        public ModuleService(InTechNetContext context)
            => _context = context;
    }
}
