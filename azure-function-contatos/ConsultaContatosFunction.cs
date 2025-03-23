using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using azure_function_contatos.Entities;

using azure_function_contatos.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace azure_function_contatos
{
    public static class ConsultaContatosFunction
    {
        private static ApplicationDbContext CreateDbContext(string connectionString)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        [FunctionName("GetAllContatos")]
        public static async Task<IActionResult> GetAllContatos(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contatos/getAll")] HttpRequest req)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString");

                // Criação do DbContext
                var dbContext = CreateDbContext(connectionString);

                // Consulta ao banco de dados
                var contacts = await dbContext.Contatos.ToListAsync();

                return new OkObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = "Contatos obtidos com sucesso",
                    HasError = false,
                    Data = contacts
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = $"Erro ao obter contatos: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }

        [FunctionName("GetByDDD")]
        public static async Task<IActionResult> GetByDDD(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contatos/getByDDD/{ddd}")] HttpRequest req,
            int ddd)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString");

                // Criação do DbContext
                var dbContext = CreateDbContext(connectionString);

                // Filtrar contatos pelo DDD
                var filteredContacts = await dbContext.Contatos
                    .Where(c => c.telefone.StartsWith($"({ddd})"))
                    .ToListAsync();

                return new OkObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = "Contatos filtrados obtidos com sucesso",
                    HasError = false,
                    Data = filteredContacts
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = $"Erro ao obter contatos filtrados: {ex.Message}",
                    HasError = true
                });
            }
        }

        [FunctionName("GetById")]
        public static async Task<IActionResult> GetById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contatos/getById/{id}")] HttpRequest req,
            Guid id)
        {
            try
            {
                var connectionString = Environment.GetEnvironmentVariable("PostgresConnectionString");

                // Criação do DbContext
                var dbContext = CreateDbContext(connectionString);

                // Consulta por ID
                var contato = await dbContext.Contatos.FirstOrDefaultAsync(c => c.id == id);

                return new OkObjectResult(new ApiResponse<ContatosResponse>
                {
                    Message = "Contato obtido com sucesso",
                    HasError = false,
                    Data = contato
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponse<ContatosResponse>
                {
                    Message = $"Erro ao obter contato por ID: {ex.Message}",
                    HasError = true
                });
            }
        }


    }
}
