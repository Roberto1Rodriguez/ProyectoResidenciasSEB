using PdfSharp.Fonts;

namespace ProyectoResidenciasApi
{

    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            if (faceName.Equals("Arial", StringComparison.OrdinalIgnoreCase))
            {
                // Carga la fuente Arial desde un archivo
                string pathToArialFontFile = "~/ARIAL.TTF"; // Ruta al archivo de fuente Arial
                return File.ReadAllBytes(pathToArialFontFile);
            }
            // Si la fuente no se encuentra, devuelve null
            return null;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Aquí puedes implementar la lógica para resolver la información de la fuente
            // Por ejemplo, puedes devolver la información de la fuente basada en el nombre de la familia y otros parámetros
            return null;
        }
    }
}
  