using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestTaskMoloko.Models;

namespace TestTaskMoloko.Contexts
{
    public sealed class AppDbContext : DbContext
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region  Properties

        public DbSet<Message> Messages { get; set; }

        public DbSet<MessageResponse> MessageSendHistory { get; set; }

        public DbSet<CriticalError> CriticalErrors { get; set; }

        #endregion

        #region Constructors

        public AppDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        #endregion

        #region Protected Methods

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connStr = _configuration.GetConnectionString("DbConnection");
            optionsBuilder.UseSqlServer(connStr);
        }

        #endregion
    }
}