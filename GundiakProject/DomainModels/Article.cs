﻿using System;
using System.ComponentModel.DataAnnotations;
using GundiakProject.DomainModels.Abstract;
using GundiakProject.Enums;
using GundiakProject.Models;

namespace GundiakProject.DomainModels
{
    public class Article : BaseEntity<int>
    {
        [Required]
        public string Title { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public byte[] Image { get; set; }
        public DateTime? DatePublished { get; set; }
        public bool WasPublished { get; set; }


        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}