using Microsoft.EntityFrameworkCore;
using TimeTracker.Core.Models;

namespace TimeTracker.Server.Data
{
   
    public class TimeTrackerDbContext : DbContext
    {
     
        public DbSet<WorkSession> WorkSessions { get; set; }

    
        public TimeTrackerDbContext(DbContextOptions<TimeTrackerDbContext> options)
            : base(options)
        {
        }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkSession>(entity =>{
                entity.ToTable("WorkSessions");
                entity.HasKey(e => e.Id);
         entity.Property(e => e.StartTime).IsRequired();
                entity.Property(e => e.EndTime).IsRequired(false);});
        }
    }
}