using ApiCoreCrudAcciones.Data;
using ApiCoreCrudAcciones.Helpers;
using ApiCoreCrudAcciones.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApiCoreCrudAcciones.Repositories
{
    public class RepositoryAcciones
    {
        private AccionesContext context;
        private HelperAccion helperAccion;

        public RepositoryAcciones(AccionesContext context, HelperAccion helperAccion)
        { 
            this.context = context;
            this.helperAccion = helperAccion;
        }

        #region ACCIONES
        public async Task<string> InsertarAccionDia()
        {

            var acciones = this.helperAccion.PedirAccionesPag();
            using (var connection = this.context.Database.GetDbConnection())
            {
                await connection.OpenAsync();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "SP_FECHA_ULTIMO_REGISTRO";

                    var consulta = await command.ExecuteScalarAsync();
                    string respuesta = consulta.ToString();
                    string[] cadena = respuesta.Split(' ');
                    string fechaBBDD = cadena[0];
                    string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");

                    if (fechaBBDD != fechaHoy)
                    {
                        foreach (var accion in acciones)
                        {
                            this.context.Acciones.Add(accion);
                        }
                        await this.context.SaveChangesAsync();
                    }

                    return fechaBBDD;
                }
            }
        }
        public async Task<List<Accion>> PedirAccionesBBDD()
        {
            string fechaHoy = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime fechaBusqueda = DateTime.ParseExact(fechaHoy, "dd/MM/yyyy", null);
            var consulta = from datos in this.context.Acciones where datos.Fecha.Date == fechaBusqueda select datos;

            return consulta.ToList();
        }
        public async Task<Accion> FindAccionAsync(int id)
        {
            return await this.context.Acciones.FirstOrDefaultAsync(z => z.ID == id);
        }

        #endregion

        #region COMPRAS
        public async Task<List<Compra>> GetCompras()
        {

            return await this.context.Compras.ToListAsync();
        }

        public async Task<int> numeroComprasAsync(int idusuario)
        {
            var consulta = from datos in this.context.Compras where (datos.idUsuairo == idusuario) select datos.idUsuairo;

            //int CantidadTotal = consulta.Sum(x=>x.Cantidad);
            int cantidad = consulta.Count();

            return cantidad;
        }
        public async Task<int> cantidadComprasAsync(int idusuario)
        {
            var consulta = from datos in this.context.Compras where (datos.idUsuairo == idusuario) select datos.Cantidad;

            //int CantidadTotal = consulta.Sum(x=>x.Cantidad);
            int cantidad = consulta.Sum();

            return cantidad;
        }


        public async Task<double> totalComprasAsync(int idusuario)
        {
            var consulta = from datos in this.context.Compras where datos.idUsuairo == idusuario select datos.Total;

            double CantidadTotal = consulta.Sum();

            return CantidadTotal;
        }

        public async Task InsertarCompraAsync(Compra compra)
        {

            string sql = "SP_INSERTAR_COMPRA @idusuario,@idaccion, @precio, @cantidad,@total";

            SqlParameter pamUsuario = new SqlParameter("@idusuario", compra.idUsuairo);
            SqlParameter pamIdAccion = new SqlParameter("@idaccion", compra.idAccion);
            SqlParameter pamPrecio = new SqlParameter("@precio", compra.Precio);
            SqlParameter pamCantidad = new SqlParameter("@cantidad", compra.Cantidad);
            SqlParameter pamTotal = new SqlParameter("@total", compra.Total);

            var consulta = this.context.Database.ExecuteSqlRaw(sql, pamUsuario, pamIdAccion, pamPrecio, pamCantidad, pamTotal);


        }

        #endregion

        #region USUARIO

        
        #endregion
    }
}
