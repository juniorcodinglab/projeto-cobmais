using Microsoft.AspNetCore.Mvc;
using Projeto_Cobmais.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projeto_Cobmais.Controllers
{
    public class CalculoDividaController : Controller
    {
        private readonly AppDbContext _context;

        /* Definindo o contexto? */
        public CalculoDividaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Exibe o formulário para upload
        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: Processa o arquivo CSV e salva os dados no banco
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("", "Por favor, selecione um arquivo CSV válido.");
                return View();
            }

            var registros = new List<ProjetoCalculoDivida>();

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var values = line.Split(',');

                    if (values.Length != 9)
                        continue;

                    // Cria um registro com os dados do CSV
                    registros.Add(new ProjetoCalculoDivida
                    {
                        Cpf = values[0],
                        Cliente = values[1],
                        Numero_Contrato = values[2],
                        Vencimento = DateTime.Parse(values[3]),
                        Valor = float.Parse(values[4]),
                        Tipo_de_Contrato = values[5],
                        Valor_Original = float.Parse(values[6]),
                        Valor_Atualizado = float.Parse(values[7]),
                        Valor_Desconto = float.Parse(values[8]),
                    });
                }
            }

            // Salva os registros no banco
            _context.CalculoDividas.AddRange(registros);
            await _context.SaveChangesAsync();

            ViewBag.Message = "Arquivo processado e dados salvos com sucesso!";
            return View();
        }

        // GET: Exibe a lista de cálculos de dívidas
        [HttpGet]
        public IActionResult Lista()
        {
            var calculoDividas = _context.CalculoDividas.ToList();
            return View(calculoDividas);
        }
    }
}
