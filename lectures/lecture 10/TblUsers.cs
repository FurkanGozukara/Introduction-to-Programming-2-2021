﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace lecture_9
{
    [Table("tblUsers")]
    [Index(nameof(UserEmail), Name = "index_userEmail", IsUnique = true)]
    public partial class TblUsers
    {
        public TblUsers()
        {
            TblTreatmentsPatientUser = new HashSet<TblTreatments>();
            TblTreatmentsPrescribingDoctorUser = new HashSet<TblTreatments>();
        }

        [Key]
        public int UserId { get; set; }
        public byte UserType { get; set; }
        [Required]
        [StringLength(200)]
        public string UserEmail { get; set; }
        [Required]
        [StringLength(64)]
        public string UserPw { get; set; }
        [Required]
        [StringLength(16)]
        public string RegisterIp { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime RegisterDate { get; set; }
        [Required]
        [StringLength(40)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(40)]
        public string Lastname { get; set; }
        [Required]
        [StringLength(36)]
        public string SaltOfPw { get; set; }

        [ForeignKey(nameof(UserType))]
        [InverseProperty(nameof(TblUserTypes.TblUsers))]
        public virtual TblUserTypes UserTypeNavigation { get; set; }
        [InverseProperty(nameof(TblTreatments.PatientUser))]
        public virtual ICollection<TblTreatments> TblTreatmentsPatientUser { get; set; }
        [InverseProperty(nameof(TblTreatments.PrescribingDoctorUser))]
        public virtual ICollection<TblTreatments> TblTreatmentsPrescribingDoctorUser { get; set; }
    }
}