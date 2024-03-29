﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace lecture_6
{
    public partial class PMSContext : DbContext
    {
        public PMSContext()
        {
        }

        public PMSContext(DbContextOptions<PMSContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer(csDbConnection.srConnectionString);
            base.OnConfiguring(builder);
        }

        public virtual DbSet<TblDiseases> TblDiseases { get; set; }
        public virtual DbSet<TblDrugs> TblDrugs { get; set; }
        public virtual DbSet<TblPrescriptions> TblPrescriptions { get; set; }
        public virtual DbSet<TblTreatments> TblTreatments { get; set; }
        public virtual DbSet<TblUserTypes> TblUserTypes { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblPrescriptions>(entity =>
            {
                entity.HasKey(e => new { e.TreatmentId, e.DrugId });

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.TblPrescriptions)
                    .HasForeignKey(d => d.DrugId)
                    .HasConstraintName("FK_tblPrescriptions_tblDrugs");

                entity.HasOne(d => d.Treatment)
                    .WithMany(p => p.TblPrescriptions)
                    .HasForeignKey(d => d.TreatmentId)
                    .HasConstraintName("FK_tblPrescriptions_tblTreatments");
            });

            modelBuilder.Entity<TblTreatments>(entity =>
            {
                entity.HasOne(d => d.Disease)
                    .WithMany(p => p.TblTreatments)
                    .HasForeignKey(d => d.DiseaseId)
                    .HasConstraintName("FK_tblTreatments_tblDiseases");

                entity.HasOne(d => d.PatientUser)
                    .WithMany(p => p.TblTreatmentsPatientUser)
                    .HasForeignKey(d => d.PatientUserId)
                    .HasConstraintName("FK_tblTreatments_tblUsers");

                entity.HasOne(d => d.PrescribingDoctorUser)
                    .WithMany(p => p.TblTreatmentsPrescribingDoctorUser)
                    .HasForeignKey(d => d.PrescribingDoctorUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblTreatments_tblUsers1");
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.Property(e => e.RegisterDate).HasDefaultValueSql("(sysutcdatetime())");

                entity.Property(e => e.RegisterIp).IsUnicode(false);

                entity.Property(e => e.UserEmail).IsUnicode(false);

                entity.Property(e => e.UserPw)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.HasOne(d => d.UserTypeNavigation)
                    .WithMany(p => p.TblUsers)
                    .HasForeignKey(d => d.UserType)
                    .HasConstraintName("FK_tblUsers_tblUserTypes");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}