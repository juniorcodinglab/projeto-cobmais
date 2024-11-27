using Microsoft.AspNetCore.Mvc;
using Projeto_Cobmais.Models;

namespace Projeto_Cobmais.Controllers
{
    public class ArquivoController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            List<ArquivoDTO> arquivos;

            try
            {
                arquivos = new ArquivoModel().RecuperarDadosArquivos();
            }
            catch (Exception ex)
            {
                arquivos = new();
                ViewData["Exception"] = $"Ocorreu um erro ao buscar os arquivos: {ex.Message}";
            }

            return View("Index", arquivos);
        }

        [HttpGet]
        public RetornoAjax<string> RecuperarConteudo(int id)
        {
            RetornoAjax<string> result = new();

            try
            {
                result.Objeto = new ArquivoModel().RecuperarConteudo(id);
                result.Status = !string.IsNullOrEmpty(result.Objeto);
            }
            catch (Exception ex)
            {
                result.Mensagem[DictionaryHelper.Inconsistencias] = $"Ocorreu um erro ao buscar o conteúdo do arquivo: {ex.Message}";
                result.Exception = true;
                throw;
            }

            return result;
        }

        [HttpPost]
        public RetornoAjax<ArquivoDTO> Importar()
        {
            RetornoAjax<ArquivoDTO> result = new();

            try
            {
                new ArquivoModel().Importar(result);
            }
            catch (Exception ex)
            {
                result.Exception = true;
                result.Mensagem[DictionaryHelper.Inconsistencias] = "Ocorreu um erro ao importar: " + ex.Message;
            }

            return result;
        }

        [Route("/Arquivo/Detalhes")]
        public IActionResult Detalhes(int id)
        {
            ArquivoDTO arquivoDTO = new();

            try
            {
                arquivoDTO = new ArquivoModel().RecuperarDadosArquivos(new int[] { id }).ElementAtOrDefault(0);

                if (arquivoDTO == null)
                    ViewData["Exception"] = "Não foi possível encontrar o arquivo";

            }
            catch (Exception ex)
            {
                ViewData["Exception"] = "Ocorreu um erro ao buscar o arquivo: " + ex.Message;
            }

            return View(arquivoDTO);
        }
    }
}
