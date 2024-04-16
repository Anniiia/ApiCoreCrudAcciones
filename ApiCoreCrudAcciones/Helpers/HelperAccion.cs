using ApiCoreCrudAcciones.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace ApiCoreCrudAcciones.Helpers
{
    public class HelperAccion
    {
        public List<Accion> PedirAccionesPag()
        {

            string urlDatos = "https://www.investing.com/";
            HtmlWeb web = new HtmlWeb();
            HtmlDocument html = web.Load(urlDatos);

            List<Accion> acciones = new List<Accion>();

            //var nodes = html.DocumentNode.CssSelect("[class='datatable-v2_body__8TXQk' tr td div a]").Select(x => x.InnerText).Distinct();
            var nodes = html.DocumentNode.CssSelect("[class='block overflow-hidden text-ellipsis whitespace-nowrap']").Select(x => x.InnerText).Distinct();
            //var nodesCap = html.DocumentNode.CssSelect("[class='datatable-v2_cell__IwP1U dynamic-table-v2_col-other__zNU4A text-right rtl:text-right'] span").Select(x => x.InnerText).Distinct();
            var nodesMax = html.DocumentNode.CssSelect("[class='datatable-v2_cell__IwP1U dynamic-table-v2_col-other__zNU4A text-right rtl:text-right']").Select(x => x.InnerText).Distinct();
            var table = html.DocumentNode.CssSelect("[class='datatable-v2_row__hkEus dynamic-table-v2_row__ILVMx'] td").Select(x => x.InnerText);
            List<string> nodesCam = new List<string>();
            List<string> nodesCamPor = new List<string>();
            for (var i = 4; i <= table.Count() - 1; i += 7)
            {
                var porcentaje = table.ElementAt(i);
                nodesCam.Add(porcentaje);
                var porcentajePor = table.ElementAt(i + 1);
                nodesCamPor.Add(porcentajePor);
            }

            int contador = 0;

            for (int i = 0; i <= 9 - 1; i++)
            {

                Accion accion = new Accion();
                //accion.Id = 0;
                accion.Nombre = nodes.ElementAt(i).Replace("&amp;", "&"); ;
                accion.Ultimo = nodesMax.ElementAt(contador);
                accion.Maximo = nodesMax.ElementAt(contador + 1);
                accion.Minimo = nodesMax.ElementAt(contador + 2);
                accion.Cambio = nodesCam.ElementAt(i);
                accion.CambioPorcentaje = nodesCamPor.ElementAt(i);
                accion.Fecha = DateTime.Now;

                contador += 3;
                acciones.Add(accion);

            }

            return acciones;

        }
        public byte[] EncryptPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA512 sha = SHA512.Create();
            //CONVERTIMOS contenido A BYTES[]
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            //CREAMOS LAS ITERACIONES
            for (int i = 1; i <= 114; i++)
            {
                salida = sha.ComputeHash(salida);
            }
            sha.Clear();
            return salida;
        }
        public static string GenerateTokenMail()
        {
            Random random = new Random();
            string token = "";
            for (int i = 1; i <= 14; i++)
            {
                //65 - 122
                int aleat = random.Next(65, 122);
                char letra = Convert.ToChar(aleat);
                token += letra;
            }
            return token;
        }

        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 1; i <= 50; i++)
            {
                int aleat = random.Next(1, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        //NECESITAMOS UN METODO PARA COMPARAR SI LOS PASSWORD SON
        //IGUALES.  DEBEMOS COMPARAR A NIVEL DE BYTE
        public bool CompareArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                for (int i = 0; i < a.Length; i++)
                {
                    //PREGUNTAMOS SI EL CONTENIDO DE CADA BYTE ES DISTINTO
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }


    }
}
