using Microsoft.AspNetCore.Mvc;
using Projeto_Cobmais.Models;
using System.Globalization;


namespace Projeto_Cobmais.Controllers
{
    public class CalculoDividaController : Controller
    {
        private readonly AppDbContext _context;

        public CalculoDividaController(AppDbContext context)
        {
            /* É usado em todo o controlador para acessar ou manipular dados no banco de dados. */
            _context = context;
        }

        // GET: Exibe o formulário para upload
        [HttpGet]
        public IActionResult Upload()
        {
            return View("~/Pages/Upload.cshtml");
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

                    // O Arquivo precisa ter 6 registros passar
                    if (values.Length != 6)
                        continue;

                    DateTime vencimento = DateTime.ParseExact(values[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    // Cria um registro com os dados do CSV
                    registros.Add(new ProjetoCalculoDivida
                    {
                        Cpf = values[0],
                        Cliente = values[1],
                        Numero_Contrato = values[2],
                        Vencimento = vencimento,
                        Valor = float.Parse(values[4]),
                        Tipo_de_Contrato = values[5],
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
