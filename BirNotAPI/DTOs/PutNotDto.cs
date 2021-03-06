using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BirNotAPI.DTOs
{
    public class PutNotDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Baslik { get; set; }
        public string Icerik { get; set; }
    }
}