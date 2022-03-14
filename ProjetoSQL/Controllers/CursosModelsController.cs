using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoSQL.Data;
using ProjetoSQL.Models;
using ProjetoSQL.Repository;

namespace ProjetoSQL.Controllers
{   [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CursosModelsController : ControllerBase
    {
        private readonly Context _context;
        private readonly IRepository repository;

        public CursosModelsController(Context context, IRepository repository)
        {
            _context = context;
            this.repository = repository;
        }
        // GET: api/CursosModels/5 Listar todos os cursos
        [AllowAnonymous]
        [HttpGet("Listar todos os cursos -- Qualquer um")]

        public async Task<ActionResult<IEnumerable<CursosModel>>> GetCursosModel2()
        {
            return await _context.Cursos.ToListAsync();
        }
        // GET: api/CursosModels/5 consultando por status
        [AllowAnonymous]
        [HttpGet("Consultar por status. -- Qualquer um")]
        public IActionResult GetCursosModel(StatusEnum Status)
        {
            var curso = repository.ObterCursosPorStatus(Status);

            return Ok(curso);
        }

        // PUT: api/CursosModels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Gerente, Secretaria")]
        [HttpPut("Alterar Status somente Gerente e secretaria")]
        public async Task<IActionResult> PutCursosModel(int id, CursosModel cursosModel)
        {
            if (id != cursosModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(cursosModel).State = EntityState.Modified;
            //Aqui é indicado que a propriedade nao deve ser alterada
            _context.Entry(cursosModel).Property(p => p.Titulo).IsModified = false;
            //Aqui é indicado que a propriedade nao deve ser alterada
            _context.Entry(cursosModel).Property(p => p.duracao).IsModified = false;



            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursosModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/CursosModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [AllowAnonymous]
        [HttpPost("Cadastrar novo curso -- qualquer um")]
        public async Task<ActionResult<CursosModel>> PostCursosModel(CursosModel cursosModel)
        {
            _context.Cursos.Add(cursosModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursosModel", new { id = cursosModel.Id }, cursosModel);
        }

        // DELETE: api/CursosModels/5
        [Authorize(Roles ="Gerente")]
        
        [HttpDelete("Somente Gerente pode deletar")]
        public async Task<IActionResult> DeleteCursosModel(int id)
        {
            var cursosModel = await _context.Cursos.FindAsync(id);
            if (cursosModel == null)
            {
                return NotFound();

            }

            _context.Cursos.Remove(cursosModel);
            await _context.SaveChangesAsync();
            return Ok("Realizado com sucesso");
        }
          //  return NoContent();

        private bool CursosModelExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}
