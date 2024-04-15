using ApiCoreCrudAcciones.Models;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
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


    }
}
