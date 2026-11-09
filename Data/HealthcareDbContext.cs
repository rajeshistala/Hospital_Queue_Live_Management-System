using HQLM_Backend.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Reflection.Emit;

namespace HQLM_Backend.Data
{
    public class HQLMDBContext:DbContext
    {
        public HQLMDBContext(
       DbContextOptions<HQLMDBContext> options)
       : base(options)
        {
        }

        public DbSet<Users> Users => Set<Users>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<Doctor> Doctors => Set<Doctor>();

        public DbSet<Patient> Patients => Set<Patient>();

        public DbSet<Appointment> Appointments => Set<Appointment>();

        public DbSet<QueueToken> QueueTokens => Set<QueueToken>();

        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<Users>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Users>()
                .Property(u => u.Email)
                .HasMaxLength(150)
                .IsRequired();

            modelBuilder.Entity<Users>()
                .Property(u => u.PasswordHash)
                .HasMaxLength(500)
                .IsRequired();

            // Role
            modelBuilder.Entity<Roles>()
                .HasKey(r => r.RoleId);

            modelBuilder.Entity<Roles>()
                .Property(r => r.RoleName)
                .HasMaxLength(50)
                .IsRequired();

            // UserRole - many-to-many relationship
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Users)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                        .HasOne(ur => ur.Roles)
                        .WithMany()
                        .HasForeignKey(ur => ur.RoleId);

            // Doctor - User relationship
            modelBuilder.Entity<Doctor>()
                .HasKey(d => d.DoctorId);

            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Users)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.UserId);

            // Patient - User relationship
            modelBuilder.Entity<Patient>()
                .HasKey(p => p.PatientId);

            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Users)
                .WithOne()
                .HasForeignKey<Patient>(p => p.UserId);

            // Appointment - Patient relationship
            modelBuilder.Entity<Appointment>()
                .HasKey(a => a.AppointmentId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId);

            // Appointment - Doctor relationship
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId);

            // Appointment - QueueToken relationship
            modelBuilder.Entity<QueueToken>()
                .HasKey(q => q.QueueTokenId);

            modelBuilder.Entity<QueueToken>()
                .HasOne(q => q.Appointment)
                .WithOne(a => a.QueueToken)
                .HasForeignKey<QueueToken>(q => q.AppointmentId);

            // Notification - User relationship
            modelBuilder.Entity<Notification>()
                .HasKey(n => n.NotificationId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Users)
                .WithMany()
                .HasForeignKey(n => n.UserId);

            // Decimal precision
            modelBuilder.Entity<Doctor>()
                .Property(d => d.ConsultationFee)
                .HasPrecision(10, 2);
        }
    }
}
