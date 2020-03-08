using InTechNet.DataAccessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace InTechNet.DataAccessLayer
{
    public class InTechNetContext : DbContext
    {

        public DbSet<Moderator> Moderators { get; set; }
        public DbSet<Pupil> Pupils { get; set; }
        public DbSet<Hub> Hubs { get; set; }
        public DbSet<Attendee> Organisators { get; set; }
        public DbSet<Organisator> Attendees { get; set; }

        public InTechNetContext(DbContextOptions<InTechNetContext> options) : base(options) { }

    }
}
