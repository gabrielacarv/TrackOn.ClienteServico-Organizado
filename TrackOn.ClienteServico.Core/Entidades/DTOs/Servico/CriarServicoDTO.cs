using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackOn.ClienteServico.Core.Entidades.DTOs.Servico
{
    public class CriarServicoDTO
    {
        public string? EnderecoUrl { get; set; }
        public int Tipo { get; set; }
        public int ClienteId { get; set; }
    }
}
