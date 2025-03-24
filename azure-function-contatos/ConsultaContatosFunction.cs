using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using azure_function_contatos.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace azure_function_contatos
{
    public static class ConsultaContatosFunction
    {

        [FunctionName("GetAllContatos")]
        public static async Task<IActionResult> GetAllContatos(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "contatos/getAll")] HttpRequest req)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:5003/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Faz a chamada para a API Gateway
                    HttpResponseMessage response = await client.GetAsync("gateway/contatos/getAll");

                    if (response.IsSuccessStatusCode)
                    {
                        // Lê e desserializa a resposta da API
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var contatos = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ContatosResponse>>>(responseContent, options);


                        return new OkObjectResult(contatos);
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                        {
                            Message = $"Erro ao consultar a API Gateway: {response.StatusCode}",
                            HasError = true,
                            Data = null
                        });
                    }
                }
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:5003/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Faz a chamada para a API Gateway
                    HttpResponseMessage response = await client.GetAsync($"gateway/Contatos/getByDDD/{ddd}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Lê e desserializa a resposta da API
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var contatos = JsonSerializer.Deserialize<ApiResponse<IEnumerable<ContatosResponse>>>(responseContent, options);

                        return new OkObjectResult(contatos);
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                        {
                            Message = $"Erro ao consultar a API Gateway: {response.StatusCode}",
                            HasError = true,
                            Data = null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponse<IEnumerable<ContatosResponse>>
                {
                    Message = $"Erro ao obter contatos filtrados: {ex.Message}",
                    HasError = true,
                    Data = null
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
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:5003/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    // Faz a chamada para a API Gateway
                    HttpResponseMessage response = await client.GetAsync($"gateway/Contatos/getById/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Lê e desserializa a resposta da API
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var options = new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        };

                        var contato = JsonSerializer.Deserialize<ApiResponse<ContatosResponse>>(responseContent, options);

                        return new OkObjectResult(contato);
                    }
                    else
                    {
                        return new BadRequestObjectResult(new ApiResponse<ContatosResponse>
                        {
                            Message = $"Erro ao consultar a API Gateway: {response.StatusCode}",
                            HasError = true,
                            Data = null
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new ApiResponse<ContatosResponse>
                {
                    Message = $"Erro ao obter contato por ID: {ex.Message}",
                    HasError = true,
                    Data = null
                });
            }
        }


    }
}
