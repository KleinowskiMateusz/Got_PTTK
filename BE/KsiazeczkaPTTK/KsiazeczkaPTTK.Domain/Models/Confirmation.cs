using KsiazeczkaPttk.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KsiazeczkaPttk.Domain.Models
{
    public class Confirmation
    {
        public int Id { get; set; }

        [Required]
        public ConfirmationType Type { get; set; }

        [Required]
        [MaxLength(250)]
        public string Url { get; set; }

        public int TerrainPointId { get; set; }

        [ForeignKey("TerrainPointId")]
        public TerrainPoint TerrainPoint { get; set; }

        public DateTime Date { get; set; }

        public bool IsAdministration { get; set; }
    }
}
