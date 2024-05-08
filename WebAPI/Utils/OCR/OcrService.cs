using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace WebAPI.Utils.OCR
{
    public class OcrService
    {
        //chave da Azure
        private readonly string _subscriptKey = "14771d3610044991b596b000471c0af5";

        //Endpoint da Azure
        private readonly string _endpoint = "https://cvvitalhubg7t.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
        {
            try
            {
                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptKey))
                {
                    Endpoint = _endpoint
                };

                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                return ProcessRecognitionResult(ocrResult);
            }
            catch (Exception ex)
            {

                return "Erro ao reconhcer o texto: " + ex.Message;
            }
        }

        private static string ProcessRecognitionResult(OcrResult result)
        {
            try
            {
                string recognizeText = "";

                foreach (var region in result.Regions)
                {
                    foreach (var line in region.Lines)
                    {
                        foreach (var word in line.Words)
                        {
                            recognizeText += word.Text + " ";
                        }
                        recognizeText += "\n";
                    }
                }
                return recognizeText;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
