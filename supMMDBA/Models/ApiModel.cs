﻿using System.ComponentModel.DataAnnotations;

namespace mmdba.Models
{
    public class ApiModel
    {

        [Required(ErrorMessage = "O campo Código do evento é obrigatório.")]
        public string CodigoEvento { get; set; } 

        [Required]
        public string Valor { get; set; }

        public string Informacao { get; set; }

        [Required]
        public string Origem { get; set; }

        [Required]
        public string TipoEvento { get; set; }

        public DateTime Timestamp { get; set; } 

    }
}
