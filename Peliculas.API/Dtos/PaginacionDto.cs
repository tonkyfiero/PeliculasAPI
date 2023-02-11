namespace Peliculas.API.Dtos
{
    public class PaginacionDto
    {
        public int Pagina { get; set; } = 1;
        private int _registrosPorPagina { get; set; } = 10;

        private int RegistrosMaximosPorPagina { get; set; } = 50;

        public int RegistrosPorPagina 
        { get 
            { return _registrosPorPagina; }
          set
            {
                _registrosPorPagina =  _registrosPorPagina > RegistrosMaximosPorPagina ? RegistrosMaximosPorPagina : value;   
            }
        }

    }
}
