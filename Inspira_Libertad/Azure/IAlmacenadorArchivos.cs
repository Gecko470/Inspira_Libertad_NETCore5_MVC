using System.Threading.Tasks;

namespace Inspira_Libertad.Azure
{
    public interface IAlmacenadorArchivos
    {
        Task<string> GuardarArchivo(byte[] contenido, string contenedor, string extension, string contentType);
        Task<string> EditarArchivo(byte[] contenido, string contenedor, string extension, string contentType, string ruta);
        Task BorrarArchivo(string contendor, string ruta);
    }
}
