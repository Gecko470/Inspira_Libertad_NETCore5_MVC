﻿using Inspira_Libertad.Data;
using Inspira_Libertad.DTOs;
using Inspira_Libertad.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Inspira_Libertad.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly ApplicationDbContext appDbContext;
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext AppDbContext, IHttpContextAccessor httpContext, 
            UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.logger = logger;
            appDbContext = AppDbContext;
            this.httpContext = httpContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            List<Frase> listaFrases = await appDbContext.Frases.ToListAsync();
            return View(listaFrases);
        }

        public IActionResult QueEs()
        {
            return View();
        }

        public IActionResult DepAdicc()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Cursos()
        {
            List<Articulo> listaArticulos = await appDbContext.Articulos.Where(p => p.Habilitado == 1).ToListAsync();
            return View(listaArticulos);
        }

        public IActionResult AtPers()
        {
            return View();
        }

        public IActionResult Retiros()
        {
            return View();
        }

        public IActionResult SobreMi()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contacto(Boolean resp = false)
        {
            ViewBag.resp = resp;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Contacto(Mensaje mensaje)
        {
            await appDbContext.Mensajes.AddAsync(mensaje);
            await appDbContext.SaveChangesAsync();

            var apiKey = configuration.GetValue<string>("InspiraLibertad_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("juangomezsousa@outlook.com", "Inspira Libertad");
            var subject = "Mensaje cliente Inspira Libertad";
            var to = new EmailAddress("codeworks9@gmail.com", "María Gómez");
            var plainTextContent = mensaje.Texto;
            var htmlContent = @$"<strong>Email: {mensaje.Email}</strong><br/><strong>Mensaje: {mensaje.Texto}</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            Response response = await client.SendEmailAsync(msg);
            return RedirectToAction("Contacto", new {resp = response.IsSuccessStatusCode});
        }

        [Authorize]
        public async Task<IActionResult> AreaPersonal()
        {
            var userId = userManager.GetUserId(httpContext.HttpContext.User);

            List<Order> listaOrdenesExistentes = await appDbContext.Orders.Where(x => x.UserId == userId && x.Status == 1).ToListAsync();
            List<int> listaCursosIds = new List<int>();
            List<Curso> listaCursos = new List<Curso>();

            if (listaOrdenesExistentes.Count > 0)
            {
                foreach (var ordenExistente in listaOrdenesExistentes)
                {
                    List<int> listaCursoIdExistentesPorOrden = await appDbContext.OrderItems.Where(x => x.OrderId == ordenExistente.OrderId).Select(x => x.CursoId).ToListAsync();

                    listaCursosIds.AddRange(listaCursoIdExistentesPorOrden);
                    listaCursos = await appDbContext.Cursos.Where(x => listaCursosIds.Contains(x.CursoId)).ToListAsync();
                }

            }
            return View(listaCursos);
        }

        [Authorize]
        public IActionResult Carrito(List<CarritoItem> cursos)
        {
            return View(cursos);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            ViewBag.ImporteTotal = await appDbContext.Orders.Where(x => x.UserId == userManager.GetUserId(httpContext.HttpContext.User)).OrderBy(x => x.OrderId).Select(x => x.ImporteTotal).LastOrDefaultAsync();
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder([FromBody] List<CarritoItem> cursos)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            Order order = new Order();
            order.UserId = userManager.GetUserId(httpContext.HttpContext.User);
            await appDbContext.Orders.AddAsync(order);
            await appDbContext.SaveChangesAsync();

            int orderId = await appDbContext.Orders.OrderBy(x => x.OrderId).Select(x => x.OrderId).LastOrDefaultAsync();
            List<OrderItem> lista = new List<OrderItem>();

            List<Order> listaOrdenesExistentes = await appDbContext.Orders.Where(x => x.UserId == order.UserId).ToListAsync();
            List<int> listaCursoIdExistentes = new List<int>();
            float importeTotal = 0;
            if (listaOrdenesExistentes.Count > 0)
            {
                foreach (var ordenExistente in listaOrdenesExistentes)
                {
                    List<int> listaCursoIdExistentesPorOrden = await appDbContext.OrderItems.Where(x => x.OrderId == ordenExistente.OrderId).Select(x => x.CursoId).ToListAsync();

                    listaCursoIdExistentes.AddRange(listaCursoIdExistentesPorOrden);

                }
                foreach (var item in cursos)
                {
                    var existe = listaCursoIdExistentes.FirstOrDefault(x => x.Equals(item.Id));
                    if (existe == 0)
                    {
                        OrderItem orderItem = new OrderItem();
                        importeTotal += item.Price;
                        orderItem.OrderId = orderId;
                        orderItem.CursoId = item.Id;
                        orderItems.Add(orderItem);
                    }
                }

                await appDbContext.OrderItems.AddRangeAsync(orderItems);
                await appDbContext.SaveChangesAsync();

                Order orderCreada = await appDbContext.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
                orderCreada.ImporteTotal = importeTotal;
                await appDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> PaymentProcess()
        {
            string userId = userManager.GetUserId(httpContext.HttpContext.User);
            Order lastOrder = await appDbContext.Orders.Where(x => x.UserId == userId).OrderBy(x => x.OrderId).LastOrDefaultAsync();
            lastOrder.Status = 1;
            await appDbContext.SaveChangesAsync();

            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
