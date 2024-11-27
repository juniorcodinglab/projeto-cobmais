namespace Projeto_Cobmais.Models
{
    using Microsoft.EntityFrameworkCore;

    public class ProjetoCalculoDivida
    {
        public string Cpf { get; set; } // Chave primária
        public string Cliente { get; set; }
        public string Numero_Contrato { get; set; }
        public DateTime Vencimento { get; set; }
        public float Valor { get; set; }
        public string Tipo_de_Contrato { get; set; }
        public float Valor_Original { get; set; }
        public float Valor_Atualizado { get; set; }
        public float Valor_Desconto { get; set; }
    }
    // Contexto do banco de dados
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet representa a tabela no banco de dados
        public DbSet<ProjetoCalculoDivida> CalculoDividas { get; set; }
    }
}
