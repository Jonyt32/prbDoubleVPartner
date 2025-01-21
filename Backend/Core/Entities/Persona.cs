namespace Core.Entities
{
    public class Persona
    {
        public int Identificador { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Email { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Identificacion { get; set; }
        public string? NombresApellidos { get; set; }
    }
}
