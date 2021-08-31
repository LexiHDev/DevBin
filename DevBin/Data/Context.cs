﻿using DevBin.Models;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace DevBin.Data
{
    public partial class Context : DbContext
    {
        public Context()
        {
        }

        public Context(DbContextOptions<Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Exposure> Exposures { get; set; }
        public virtual DbSet<Paste> Pastes { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Syntaxes> Syntaxes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8mb4")
                .UseCollation("utf8mb4_general_ci");

            modelBuilder.Entity<Exposure>(entity =>
            {
                entity.ToTable("exposures");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AllowEdit).HasColumnName("allowEdit");

                entity.Property(e => e.IsPrivate).HasColumnName("isPrivate");

                entity.Property(e => e.IsPublic).HasColumnName("isPublic");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("name");

                entity.Property(e => e.RegisteredOnly).HasColumnName("registeredOnly");
            });

            modelBuilder.Entity<Paste>(entity =>
            {
                entity.ToTable("pastes");

                entity.HasIndex(e => e.AuthorId, "FK_pastes_Author_ToUsers");

                entity.HasIndex(e => e.ExposureId, "FK_pastes_Exposure_ToExposures");

                entity.HasIndex(e => e.SyntaxId, "FK_pastes_Syntax_ToSyntaxes");

                entity.HasIndex(e => e.Code, "code")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.AuthorId)
                    .HasColumnType("int(11)")
                    .HasColumnName("authorId");

                entity.Property(e => e.Cache)
                    .HasMaxLength(255)
                    .HasColumnName("cache");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("code");

                entity.Property(e => e.Datetime)
                    .HasMaxLength(6)
                    .HasColumnName("datetime")
                    .HasDefaultValueSql("current_timestamp(6)");

                entity.Property(e => e.ExposureId)
                    .HasColumnType("int(11)")
                    .HasColumnName("exposureId")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.SyntaxId)
                    .HasColumnType("int(11)")
                    .HasColumnName("syntaxId")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .HasColumnName("title")
                    .HasDefaultValueSql("'''Unnamed Paste'''");

                entity.Property(e => e.UpdateDatetime)
                    .HasMaxLength(6)
                    .HasColumnName("updateDatetime");

                entity.Property(e => e.Views)
                    .HasColumnType("int(11)")
                    .HasColumnName("views");

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Pastes)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_pastes_Author_ToUsers");

                entity.HasOne(d => d.Exposure)
                    .WithMany(p => p.Pastes)
                    .HasForeignKey(d => d.ExposureId)
                    .HasConstraintName("FK_pastes_Exposure_ToExposures");

                entity.HasOne(d => d.Syntax)
                    .WithMany(p => p.Pastes)
                    .HasForeignKey(d => d.SyntaxId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK_pastes_Syntax_ToSyntaxes");
            });

            modelBuilder.Entity<Session>(entity =>
            {
                entity.ToTable("sessions");

                entity.HasIndex(e => e.UserId, "IX_Sessions_UserId");

                entity.Property(e => e.Id).HasColumnType("int(11)");

                entity.Property(e => e.Datetime)
                    .HasMaxLength(6)
                    .HasDefaultValueSql("current_timestamp(6)");

                entity.Property(e => e.UserId).HasColumnType("int(11)");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Sessions)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Sessions_users_UserId");
            });

            modelBuilder.Entity<Syntaxes>(entity =>
            {
                entity.ToTable("syntaxes");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("name");

                entity.Property(e => e.Pretty)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("pretty");

                entity.Property(e => e.Show).HasColumnName("show");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.ApiToken, "KEY_apiToken_UQ")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "KEY_username_UQ")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "email_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.ApiToken)
                    .HasMaxLength(256)
                    .HasColumnName("apiToken")
                    .UseCollation("utf8mb4_bin");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("password")
                    .IsFixedLength(true);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("username");

                entity.Property(e => e.Verified).HasColumnName("verified");

                entity.Property(e => e.VerifyCode)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("verifyCode");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
