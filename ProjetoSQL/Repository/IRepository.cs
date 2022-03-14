using ProjetoSQL.Models;
using System.Collections.Generic;

namespace ProjetoSQL.Repository
{
    public interface IRepository
    {

        IEnumerable<CursosModel> ObterTodosCursos();
        IEnumerable<CursosModel> ObterCursosPorStatus(StatusEnum Status);

    }
}