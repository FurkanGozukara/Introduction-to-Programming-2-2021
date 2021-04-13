﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace lecture_6
{
    [Table("tblTreatments")]
    public partial class TblTreatments
    {
        public TblTreatments()
        {
            TblPrescriptions = new HashSet<TblPrescriptions>();
        }

        [Key]
        public int TreatmentId { get; set; }
        public int PrescribingDoctorUserId { get; set; }
        public int PatientUserId { get; set; }
        public int DiseaseId { get; set; }

        [ForeignKey(nameof(DiseaseId))]
        [InverseProperty(nameof(TblDiseases.TblTreatments))]
        public virtual TblDiseases Disease { get; set; }
        [ForeignKey(nameof(PatientUserId))]
        [InverseProperty(nameof(TblUsers.TblTreatmentsPatientUser))]
        public virtual TblUsers PatientUser { get; set; }
        [ForeignKey(nameof(PrescribingDoctorUserId))]
        [InverseProperty(nameof(TblUsers.TblTreatmentsPrescribingDoctorUser))]
        public virtual TblUsers PrescribingDoctorUser { get; set; }
        [InverseProperty("Treatment")]
        public virtual ICollection<TblPrescriptions> TblPrescriptions { get; set; }
    }
}