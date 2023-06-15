using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ConsumindoApi
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int op;
            string BaseUrl = "http://localhost:5038";

            do
            {
                Console.WriteLine(" ");
                Console.WriteLine("Informe a opção desejada:");
                Console.WriteLine(" ");

                Console.WriteLine("1 - Consultar pessoas");
                Console.WriteLine("2 - Cadastrar pessoas");
                Console.WriteLine("3 - Alterar pessoas");
                Console.WriteLine("4 - Excluir pessoas");
                Console.WriteLine(" ");

                op = int.Parse(Console.ReadLine());

                Console.Clear();

                switch (op)
                {
                    case 0:
                        break;

                    case 1:
                        // Recebe o que tem armazenado no sistema
                        List<Pessoa> pessoas = new List<Pessoa>();

                        // Instancia um objeto para fazer o acesso via HTTP
                        HttpClient client = new HttpClient();

                        // Define o endereço da API
                        client.BaseAddress = new Uri(BaseUrl);
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers
                            .MediaTypeWithQualityHeaderValue("application/json"));
       
                        // Acessa o EndPoint
                        HttpResponseMessage resposta = await client.GetAsync("api/Pessoa/pessoas");

                        if (resposta.IsSuccessStatusCode)
                        {
                            // Pega a resposta
                            var retorno = resposta.Content.ReadAsStringAsync().Result;
                            // Pega o retorno e desserializa na lista
                            pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(retorno);
                        }
                        else
                        {
                            Console.WriteLine($"Erro: {resposta.StatusCode}");
                        }
                        foreach (Pessoa p in pessoas)
                        {
                            Console.WriteLine($"Id: {p.id} \n" +
                                $"Nome: {p.nome}");
                        }
                        break;

                    case 2:
                        Pessoa pessoa = new Pessoa();

                        Console.WriteLine("Digite o nome da pessoa:");
                        pessoa.nome = Console.ReadLine();

                        HttpClient cliente = new HttpClient();

                        HttpResponseMessage respostaPost = await cliente
                            .PostAsJsonAsync(BaseUrl + "/api/Pessoa/pessoas", pessoa);

                         Console.WriteLine($"Retorno: {respostaPost.StatusCode}");
                        break;

                    case 3:
                        int idPut;

                        Console.WriteLine("Digite o ID para ser alterado: ");
                        idPut = int.Parse(Console.ReadLine());

                        Pessoa pessoaPut = new Pessoa();

                        Console.WriteLine("Digite o nome da pessoa: ");
                        pessoaPut.nome = Console.ReadLine();

                        HttpClient clientePut = new HttpClient();
                        HttpResponseMessage respostaPut = await clientePut
                            .PutAsJsonAsync(BaseUrl + "/api/Pessoa/pessoas/" + idPut, pessoaPut);

                        Console.WriteLine("Retorno: " + respostaPut.StatusCode);
                        break;

                    case 4:
                        int idP;

                        Console.WriteLine("Digite o ID para ser excluido:");
                        idP = int.Parse(Console.ReadLine());

                        HttpClient clienteDelete = new HttpClient();
                        HttpResponseMessage respostaDelete = await clienteDelete
                            .DeleteAsync(BaseUrl + "/api/Pessoa/pessoas/" + idP);
                        
                        Console.WriteLine("Retorno: " + respostaDelete.StatusCode);
                        break;
                }


            } while (op != 0);
        }
    }
}