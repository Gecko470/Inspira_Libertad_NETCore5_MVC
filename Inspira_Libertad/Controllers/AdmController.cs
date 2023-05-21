using AutoMapper;
using Inspira_Libertad.Azure;
using Inspira_Libertad.Data;
using Inspira_Libertad.DTOs;
using Inspira_Libertad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Inspira_Libertad.Controllers
{
    public class AdmController : Controller
    {
        private readonly ApplicationDbContext appDbContext;
        private readonly IMapper mapper;
        private readonly IAlmacenadorArchivos almacenadorArchivos;
        private readonly string contenedor = "fotos";
        private readonly string contenedor2 = "videos";

        public AdmController(ApplicationDbContext appDbContext, IMapper mapper, IAlmacenadorArchivos almacenadorArchivos)
        {
            this.appDbContext = appDbContext;
            this.mapper = mapper;
            this.almacenadorArchivos = almacenadorArchivos;
        }

        public async Task<IActionResult> Frases()
        {
            List<Frase> listaFrases = await appDbContext.Frases.ToListAsync();
            return View(listaFrases);
        }

        [HttpGet]
        public IActionResult FraseCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FraseCreate(string texto)
        {
            Frase frase = new Frase();
            frase.Texto = texto;

            if (ModelState.IsValid)
            {
                await appDbContext.Frases.AddAsync(frase);
                await appDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> FraseEdit(int id)
        {
            Frase frase = await appDbContext.Frases.FindAsync(id);
            if (frase is not null)
            {
                return View(frase);
            }
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> FraseUpdate(Frase data)
        {
            Frase frase = await appDbContext.Frases.FindAsync(data.Id);
            if (frase is not null)
            {
                frase.Texto = data.Texto;
                await appDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> FraseDelete(int id)
        {
            Frase frase = await appDbContext.Frases.FindAsync(id);
            if (frase is not null)
            {
                appDbContext.Frases.Remove(frase);
                await appDbContext.SaveChangesAsync();
            }
            return Ok();
        }

        public async Task<IActionResult> Articulos()
        {
            List<Articulo> listaArticulos = await appDbContext.Articulos.ToListAsync();
            return View(listaArticulos);
        }

        [HttpGet]
        public IActionResult ArticuloCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ArticuloCreate([FromForm] ArticuloDTO articuloDTO)
        {
            Articulo articulo = mapper.Map<Articulo>(articuloDTO);
            if (articuloDTO.File != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articuloDTO.File.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(articuloDTO.File.FileName);
                    articulo.Url = await almacenadorArchivos.GuardarArchivo(contenido, contenedor, extension, articuloDTO.File.ContentType);
                }
            }

            articulo.Habilitado = 0;
            await appDbContext.Articulos.AddAsync(articulo);
            await appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> ArticuloEdit(int id)
        {
            Articulo articulo = await appDbContext.Articulos.FirstOrDefaultAsync(p => p.ArticuloId == id);
            if (articulo != null)
            {
                ArticuloDTO articuloDTO = mapper.Map<ArticuloDTO>(articulo);
                return View(articuloDTO);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ArticuloUpdate([FromForm] ArticuloDTO articuloDTO)
        {

            //appDbContext.Entry(data).State = EntityState.Modified;
            Articulo articuloBD = await appDbContext.Articulos.FirstOrDefaultAsync(p => p.ArticuloId == articuloDTO.ArticuloId);
            if (articuloBD == null)
            {
                return NotFound();
            }

            mapper.Map(articuloDTO, articuloBD);

            if (articuloDTO.File != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await articuloDTO.File.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(articuloDTO.File.FileName);
                    articuloBD.Url = await almacenadorArchivos.EditarArchivo(contenido, contenedor, extension, articuloDTO.File.ContentType, articuloDTO.Url);
                }
            }

            await appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ArticuloDelete(int id)
        {
            Articulo articuloBD = await appDbContext.Articulos.FirstOrDefaultAsync(p => p.ArticuloId == id);
            if (articuloBD != null)
            {
                appDbContext.Remove(articuloBD);
                await appDbContext.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Videos()
        {
            List<Curso> listaCursos = await appDbContext.Cursos.ToListAsync();
            return View(listaCursos);

        }

        [HttpGet]
        public IActionResult VideoCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VideoCreate([FromForm] CursoDTO cursoDTO)
        {
            Curso curso = mapper.Map<Curso>(cursoDTO);
            if (cursoDTO.File != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await cursoDTO.File.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(cursoDTO.File.FileName);
                    curso.Url = await almacenadorArchivos.GuardarArchivo(contenido, contenedor2, extension, cursoDTO.File.ContentType);
                }
            }
            await appDbContext.Cursos.AddAsync(curso);
            await appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> VideoEdit(int id)
        {
            Curso curso = await appDbContext.Cursos.FirstOrDefaultAsync(p => p.CursoId == id);
            if (curso != null)
            {
                CursoDTO cursoDTO = mapper.Map<CursoDTO>(curso);
                return View(cursoDTO);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> VideoUpdate([FromForm] CursoDTO cursoDTO)
        {
            Curso cursoBD = await appDbContext.Cursos.FirstOrDefaultAsync(p => p.CursoId == cursoDTO.CursoId);
            if (cursoBD == null)
            {
                return NotFound();
            }

            mapper.Map(cursoDTO, cursoBD);

            if (cursoDTO.File != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await cursoDTO.File.CopyToAsync(memoryStream);
                    var contenido = memoryStream.ToArray();
                    var extension = Path.GetExtension(cursoDTO.File.FileName);
                    cursoBD.Url = await almacenadorArchivos.EditarArchivo(contenido, contenedor2, extension, cursoDTO.File.ContentType, cursoDTO.Url);
                }
            }

            await appDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> VideoDelete(int id)
        {
            Curso cursoBD = appDbContext.Cursos.Where(p => p.CursoId == id).FirstOrDefault();
            if(cursoBD == null)
            {
                return NotFound();  
            }
            else
            {
                appDbContext.Cursos.Remove(cursoBD);
                await appDbContext.SaveChangesAsync();
                return RedirectToAction("Videos");
            }
        }
    }
    
}
