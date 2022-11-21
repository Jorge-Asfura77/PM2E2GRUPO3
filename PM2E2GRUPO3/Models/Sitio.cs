using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E2GRUPO3.Models
{
    public class Sitio
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("dsc")]
        public string Descripcion { get; set; }
        [JsonProperty("lat")]
        public string Latitud { get; set; }
        [JsonProperty("lng")]
        public string Longitud { get; set; }
        [JsonProperty("firma")]
        public byte[] FirmaDigital { get; set; }
        [JsonProperty("audio")]
        public byte[] AudioFile { get; set; }
        [JsonProperty("trazado")]
        public string firma { get; set; }

        public Byte[] foto { get; set; }
    }
}
