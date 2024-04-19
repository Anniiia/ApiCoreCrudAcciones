using ApiCoreCrudAcciones.Data;
using ApiCoreCrudAcciones.Helpers;
using ApiCoreCrudAcciones.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Numerics;

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
            List<Accion> acciones = await (from datos in this.context.Acciones where datos.Fecha.Date == fechaBusqueda select datos).ToListAsync();

            return acciones;
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

        public async Task<List<Compra>> ListadoComprasUser(int idusuario)
        {
            var consulta = from datos in this.context.Compras where (datos.idUsuairo == idusuario) select datos;

            List<Compra> compras = await consulta.ToListAsync();

            return compras;

        }

        public async Task InsertarCompraAsync(int idusuario, int idaccion, double precio, int cantidad, double total)
        {

            //string sql = "SP_INSERTAR_COMPRA @idusuario,@idaccion, @precio, @cantidad,@total";

            //SqlParameter pamUsuario = new SqlParameter("@idusuario", compra.idUsuairo);
            //SqlParameter pamIdAccion = new SqlParameter("@idaccion", compra.idAccion);
            //SqlParameter pamPrecio = new SqlParameter("@precio", compra.Precio);
            //SqlParameter pamCantidad = new SqlParameter("@cantidad", compra.Cantidad);
            //SqlParameter pamTotal = new SqlParameter("@total", compra.Total);

            //var consulta = this.context.Database.ExecuteSqlRaw(sql, pamUsuario, pamIdAccion, pamPrecio, pamCantidad, pamTotal);
            Compra comp = new Compra();
            comp.idUsuairo = idusuario;
            comp.idAccion = idaccion;
            comp.Precio = precio;
            comp.Cantidad = cantidad;
            comp.Total = total;
            this.context.Compras.Add(comp);
            await this.context.SaveChangesAsync();


        }

        #endregion
        public async Task<Usuario> LogInUsuarioAsync(string email, string password)
        { 
           Usuario user = await this.context.Usuarios.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }
            else
            {
                string salt = user.Salt;
                byte[] temp = this.helperAccion.EncryptPassword(password,salt);
                byte[] passUser = user.Password;
                bool response = this.helperAccion.CompareArrays(temp,passUser);
                if (response == true)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            
        }

        public async Task RegisterUsuarioAsync (string nombre, string email, string password)
        { 
            Usuario user = new Usuario();
            user.IdUsuario = await this.GetMaxIdUsuarioAsync();
            user.Nombre = nombre;
            user.Email = email;
            user.Salt = HelperAccion.GenerateSalt();
            user.Password = helperAccion.EncryptPassword(password, user.Salt);
            user.TokenMail = "";
            this.context.Usuarios.Add(user);
            await this.context.SaveChangesAsync();

        }

        private async Task<int> GetMaxIdUsuarioAsync()
        {
            if (this.context.Usuarios.Count() == 0)
            {
                return 1;
            }
            else
            {
                return await this.context.Usuarios.MaxAsync(z => z.IdUsuario) + 1;
            }
        }
        #region USUARIO


        #endregion
    }
}
