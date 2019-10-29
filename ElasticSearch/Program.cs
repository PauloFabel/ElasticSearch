using ElasticSearch.Modulos;
using Nest;
using System;
using System.Threading.Tasks;

namespace ElasticSearch
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string url = "http://localhost:9200/";
            string defaultIndex = "agenda";

            var settings = new ConnectionSettings(new Uri(url))
            .DefaultIndex(defaultIndex)
            .DefaultMappingFor<Pessoa>(m => m);

            var client = new ElasticClient(settings);

            Processar(client).Wait();
        }

        private static async Task Processar(ElasticClient elasticClient)
        {
            Pessoa pessoa = new Pessoa()
            {
                Id = 1,
                Nome = "Paulo",
                Cidade = "Florianópolis",
                Estado = "Santa Catarina"
            };

            await elasticClient.IndexDocumentAsync(pessoa);

            pessoa = await Consultar(elasticClient, 1);

            Console.WriteLine("Encontrou {0}", pessoa != null);

            await Atualizar(elasticClient);

            pessoa = await Consultar(elasticClient, 1);

            Console.WriteLine("Encontrou {0}", pessoa != null);
            Console.WriteLine(pessoa.Nome);

            await Deletar(elasticClient);
        }

        private static async Task<Pessoa> Consultar(ElasticClient elasticClient, int id)
        {
            var response = await elasticClient.GetAsync(DocumentPath<Pessoa>.Id(new Id(id)));

            if (response.Found)
                return response.Source;

            return null;
        }

        private static async Task Deletar(ElasticClient elasticClient)
        {
            var pessoa = await Consultar(elasticClient, 1);

            await elasticClient.DeleteAsync<Pessoa>(pessoa);
        }

        private static async Task Atualizar(ElasticClient elasticClient)
        {
            var pessoa = await Consultar(elasticClient, 1);

            pessoa.Nome += " Fabel";

            await elasticClient.IndexDocumentAsync(pessoa);
        }
    }
}