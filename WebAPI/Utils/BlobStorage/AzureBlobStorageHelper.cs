using Azure.Storage.Blobs;

namespace WebAPI.Utils.BlobStorage
{
    //Uma classe estatica você consegue acessar os metodos delas sem instanciar os objetos
    public static class AzureBlobStorageHelper
    {

        public static async Task<string> UploadImageBlobAsync(IFormFile arquivo, string stringConexao, string nomeContainer)
        {

            try
            {
                //Verifica se existe um arquivo 
                if (arquivo != null)
                {
                    //Gera um nome único + extensão do arquivo
                    var blobName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(arquivo.FileName);

                    //Cria uma instancia do client Blob Service e passa a string de conexão
                    var blobServiceClient = new BlobServiceClient(stringConexao);

                    //Obtem um client usando o nome do container do blob
                    var blobContainerClient = blobServiceClient.GetBlobContainerClient(nomeContainer);

                    //Obtem um blob client usando o blob name
                    var blobClient = blobContainerClient.GetBlobClient(blobName);

                    //Abre o fluxo de entrada do arquivo(foto)
                    using (var stream = arquivo.OpenReadStream())
                    {
                        //Carrega o aquivo(foto) para o blob storage de forma assíncrona
                        await blobClient.UploadAsync(stream, true);

                    }
                    //Retorna a uri do blob como uma string
                        return blobClient.Uri.ToString();
                }
                else
                {
                    //URL da imagem padrão
                    //Retorna a uri de uma imagem padrão caso nenhum arquivo seja enviado
                    return "Imagem Padrão";
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
