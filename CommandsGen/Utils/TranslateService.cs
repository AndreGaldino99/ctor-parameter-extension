using System.Net.Http;

namespace VSIXProject1.CommandsGen.Utils
{
    public static class TranslateService
    {
        public static string TraduzirMyMemory(string texto, string idiomaDestino)
        {
            string url = $"https://api.mymemory.translated.net/get?q={Uri.EscapeDataString(texto)}&langpair=pt|{idiomaDestino}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    // Processar a resposta, por exemplo, usando JSON.NET
                    // Aqui, estamos apenas retornando o texto traduzido diretamente
                    return responseBody;
                }
                else
                {
                    // Lidar com erros de solicitação, se necessário
                    return "Erro na solicitação de tradução";
                }
            }
        }
    }
}