using System;
using GundiakProject.DomainModels.Abstract;
using GundiakProject.Enums;
using GundiakProject.Models;

namespace GundiakProject.DomainModels
{
    public class Article : BaseEntity<int>
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DatePublished { get; set; }


        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}