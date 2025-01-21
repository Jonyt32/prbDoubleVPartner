namespace Core.Entities
{
    public class Usuario
    {
        public int Identificador { get; set; }
        public string NombreUsuario { get; set; }
        public string Contrasena { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
