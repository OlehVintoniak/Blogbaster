using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GundiakProject.DomainModels.Abstract;
using GundiakProject.Enums;
using GundiakProject.Models;

namespace GundiakProject.DomainModels
{
    public class Article : BaseEntity<int>
    {
        [Required]
        [DisplayName("Заголовок")]
        public string Title { get; set; }
        [DisplayName("Текст")]
        public string Text { get; set; }
        [DisplayName("Статус")]
        public Status Status { get; set; }
        [DisplayName("Дата створення")]
        public DateTime DateCreated { get; set; }
        [DisplayName("Зображення")]
        public byte[] Image { get; set; }
        public DateTime? DatePublished { get; set; }
        public bool WasPublished { get; set; }


        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}