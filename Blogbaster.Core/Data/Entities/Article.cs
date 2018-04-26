using Blogbaster.Core.Data.Entities.Abstract;
using Blogbaster.Core.Data.Enums;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blogbaster.Core.Data.Entities
{
    public class Article : Entity<int>
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
