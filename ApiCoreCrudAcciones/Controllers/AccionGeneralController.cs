using ApiCoreCrudAcciones.Helpers;
using ApiCoreCrudAcciones.Models;
using ApiCoreCrudAcciones.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;

//codigo para hacer un login para una api que tenga jwt y authorize cn c#

namespace ApiCoreCrudAcciones.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionGeneralController : ControllerBase
    {
        private RepositoryAcciones repo;
        private HelperAccion helperAccion;
        private HelperActionServicesOAuth helper;


        public AccionGeneralController(RepositoryAcciones repo, HelperAccion helperAccion, HelperActionServicesOAuth helper)
        {
            this.repo = repo;
            this.helperAccion = helperAccion;
            this.helper = helper;
        }
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ListaAcciones()
        {
            //var acciones = this.helperAccion.InsertarAccionDia(url);
            var acciones = this.helperAccion.PedirAccionesPag();
            //this.repo.InsertarAccionDia();


            return Ok(acciones); ;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> ListaAccionesBBDD()
        {
            //this.repo.InsertarAccionDia();
            //var acciones = this.helperAccion.InsertarAccionDia(url);
            List<Accion> acciones = await this.repo.PedirAccionesBBDD();
            


            return Ok(acciones); ;
        }
        [Authorize]
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<ActionResult<Accion>> FindAccion(int id)
        {
            return await this.repo.FindAccionAsync(id);
        }

        //[HttpGet]
        //[Route("[action]")]
        //public async Task<ActionResult<List<Compra>>> GetCompras()
        //{
        //    return await this.repo.GetCompras();
        //}
        //[HttpGet("[action]/{id}")]
        //public async Task<ActionResult<int>> GetCompraUsuario(int id)
        //{
        //    return await this.repo.numeroComprasAsync(id);
        //}
        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<double>> GetTotalGanancias()
        {
            //int usuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int usuario = 1;
            double totalGananciasCompras = await this.repo.totalComprasAsync(usuario);
            return totalGananciasCompras;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<double>> GetTotalInvertido()
        {
            //int usuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int usuario = 1;
            double totalInvertidoCompras = await this.repo.totalComprasAsync(usuario);
            return totalInvertidoCompras;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> GetComprasRealizadas()
        {
            // usuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int usuario = 1;
            int totalNumeroCompras = await this.repo.numeroComprasAsync(usuario);
            return totalNumeroCompras;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<int>> GetTotalAcciones()
        {
            //int usuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int usuario = 1;
            int totalCompras = await this.repo.cantidadComprasAsync(usuario);
            return totalCompras;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<double>> GetResumen()
        {
            //int usuario = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            int usuario = 1;
            double totalCompras = await this.repo.totalComprasAsync(usuario);
            return totalCompras;
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> PostCompra(Compra compra)
        {
            await this.repo.InsertarCompraAsync(compra.idUsuairo, compra.idAccion, compra.Precio, compra.Cantidad, compra.Total);

            return Ok();
        }
        //[HttpGet("CompraCompleta")]
        //public IActionResult CompraCompleta()
        //{
        //    return Ok(new { MENSAJE = "compra realizada" });
        //}

        //[AuthorizeUsuarios]
        //[HttpGet("Pendientes")]
        //public IActionResult GetPendientes()
        //{
        //    List<Accion> acciones = new List<Accion>();
        //    //if (this.memoryCache.Get<List<Accion>>("PENDIENTES") != null)
        //    //{
        //    //    acciones = this.memoryCache.Get<List<Accion>>("PENDIENTES").ToList();
        //    //}
        //    return Ok(acciones);
        //}

        ////[AuthorizeUsuarios]
        //[HttpPost("Pendientes")]
        ////[ValidateAntiForgeryToken]
        //public IActionResult AddPendiente(int? idaccion)
        //{
        //    return RedirectToAction("PedidoFinal", idaccion);
        //}

        //[AuthorizeUsuarios]
        //[HttpGet("Productos")]
        //public async Task<IActionResult> GetProductos(int? addpendiente, int? ideliminar)
        //{
        //    List<Accion> acciones = await this.repo.PedirAccionesBBDD();
        //    if (addpendiente != null)
        //    {
        //        List<Accion> accionesPendientes;
        //        //if (this.memoryCache.Get("PENDIENTES") == null)
        //        //{
        //        //    accionesPendientes = new List<Accion>();
        //        //}
        //        //else
        //        //{
        //        //    accionesPendientes = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //        //}
        //        //Accion accionPendiente = await this.repo.FindAccionAsync(addpendiente.Value);
        //        //accionesPendientes.Add(accionPendiente);
        //        //this.memoryCache.Set("PENDIENTES", accionesPendientes);
        //        //ViewData["FAVS"] = this.memoryCache.Get<List<Accion>>("PENDIENTES");

        //    }
        //    //List<Accion> accionesTodas = await this.repo.PedirAccionesBBDD();
        //    //return View(accionesTodas);
        //    //if (ideliminar != null)
        //    //{
        //    //    List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //    //    Accion accion = accionesP.Find(z => z.ID == ideliminar.Value);
        //    //    accionesP.Remove(accion);

        //    //    if (acciones.Count == 0)
        //    //    {
        //    //        this.memoryCache.Remove("PENDIENTES");
        //    //        this.memoryCache.Remove("TOTAL");
        //    //    }
        //    //    else
        //    //    {
        //    //        this.memoryCache.Set("PENDIENTES", accionesP);
        //    //    }

        //    //}
        //    //if (this.memoryCache.Get("PENDIENTES") != null)
        //    //{
        //    //    List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //    //    int suma = accionesP.Count();
        //    //    this.memoryCache.Set("TOTAL", suma);
        //    //    ViewData["FAVS"] = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //    //}
        //    string prueba = await this.repo.InsertarAccionDia();
        //    List<Accion> accionesTodas = await this.repo.PedirAccionesBBDD();
        //    return Ok(accionesTodas);
        //}

        ////[AuthorizeUsuarios]
        //[HttpPost("Productos")]
        //[ValidateAntiForgeryToken]
        //public IActionResult Producto(int? idaccion, int? addpendiente, int? ideliminar)
        //{
        //    return RedirectToAction("PedidoFinal", idaccion);
        //}

        //[HttpGet("PedidoFinal/{idaccion}")]
        //public async Task<IActionResult> GetPedidoFinal(int idaccion)
        //{
        //    Accion accion = await this.repo.FindAccionAsync(idaccion);

        //    //this.memoryCache.Set("ACCION", accion);

        //    return Ok(accion);
        //}

        //[HttpPost("PedidoFinal/{idaccion}")]
        //public async Task<IActionResult> PedidoFinal(int idaccion, int precio, string cantidad, int total)
        //{
        //    if (HttpContext.Session.GetString("USUARIO") == null && this.memoryCache.Get("ACCION") == null)
        //    {
        //        return RedirectToAction("ListaAcciones", "AccionGeneral");

        //    }
        //    else if (this.memoryCache.Get("ACCION") == null)
        //    {
        //        return RedirectToAction("Productos", "Tienda");
        //    }
        //    else
        //    {

        //        Compra compra = new Compra();
        //        Accion accion = ((Accion)this.memoryCache.Get("ACCION"));
        //        //compra.idUsuairo = int.Parse(HttpContext.Session.GetString("IDUSUARIO"));
        //        compra.idUsuairo = int.Parse(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        //        compra.idAccion = accion.ID;
        //        //compra.Precio = double.Parse(accion.Ultimo);
        //        string texto = accion.Ultimo.Replace('.', ',');

        //        string textoOriginal = texto;
        //        int indiceComa = textoOriginal.IndexOf(',');
        //        string textoModificado = "";

        //        if (indiceComa != -1)
        //        {
        //            textoModificado = textoOriginal.Remove(indiceComa, 1);
        //            // textoModificado será "38722,69"
        //        }

        //        compra.Precio = double.Parse(textoModificado);
        //        compra.Cantidad = int.Parse(cantidad);
        //        compra.Total = double.Parse(textoModificado) * double.Parse(cantidad);

        //        this.memoryCache.Set("COMPRA", accion);

        //        await this.repo.InsertarCompraAsync(compra);


        //        this.memoryCache.Remove("ACCION");
        //        //al realizarse la compra, se borra los datos en cache de esa accion y se bloque la entrada a esta vista, se redireccion a la lista de
        //        if (this.memoryCache.Get("PENDIENTES") != null)
        //        {
        //            List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //            Accion accioP = accionesP.Find(z => z.ID == accion.ID);
        //            accionesP.Remove(accioP);
        //            this.memoryCache.Set("PENDIENTES", accionesP);
        //            return RedirectToAction("CompraCompleta", "Tienda");
        //        }


        //        //List<Accion> accionesP = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //        //Accion accioP = accionesP.Find(z => z.ID == accion.ID);
        //        //accionesP.Remove(accioP);
        //        //this.memoryCache.Set("PENDIENTES", accionesP);
        //        return RedirectToAction("CompraCompleta", "Tienda");

        //    }
        //}

        ////[AuthorizeUsuarios]
        //[HttpGet("AccionesPendientes")]
        //public IActionResult GetAccionesPendientes(int? ideliminar)
        //{
        //    if (ideliminar != null)
        //    {
        //        List<Accion> acciones = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //        Accion accion = acciones.Find(z => z.ID == ideliminar.Value);
        //        acciones.Remove(accion);

        //        if (acciones.Count == 0)
        //        {
        //            this.memoryCache.Remove("PENDIENTES");
        //            this.memoryCache.Remove("TOTAL");
        //        }
        //        else
        //        {
        //            this.memoryCache.Set("PENDIENTES", acciones);
        //        }

        //    }
        //    if (this.memoryCache.Get("PENDIENTES") != null)
        //    {
        //        List<Accion> acciones = this.memoryCache.Get<List<Accion>>("PENDIENTES");
        //        int suma = acciones.Count();
        //        this.memoryCache.Set("TOTAL", suma);
        //    }

        //    return View();
        //}


        //USUARIO Y QUE RECIBIRA LoginModel
        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult> Login(LoginModel model)
        {
            //BUSCAMOS AL EMPLEADO EN NUESTRO REPO
            Usuario usuario =
                await this.repo.LogInUsuarioAsync
                (model.UserName, model.Password);
            if (usuario == null)
            {
                return Unauthorized();
            }
            else
            {

                SigningCredentials credentials =
                    new SigningCredentials(
                        this.helper.GetKeyToken()
                        , SecurityAlgorithms.HmacSha256);

                JwtSecurityToken token =
                    new JwtSecurityToken(
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );

                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler()
                        .WriteToken(token)
                    });

            }

        }

        [HttpPost]
        [Route("[action]")]

        public async Task<ActionResult> InsertUsuario(string nombre, string email, string password)
        { 
            await this.repo.RegisterUsuarioAsync(nombre, email, password);

            return Ok();


        }
        

    }
}

