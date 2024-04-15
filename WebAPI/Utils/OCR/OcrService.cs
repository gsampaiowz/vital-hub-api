using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace WebAPI.Utils.OCR
    {
    public class OcrService
        {
        private readonly string _subscriptionKey = "14771d3610044991b596b000471c0af5";

        private readonly string _endpoint = "https://cvvitalhubg7t.cognitiveservices.azure.com/";

        public async Task<string> RecognizeTextAsync(Stream imageStream)
            {
            try
                {
                var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_subscriptionKey))
                    {
                    Endpoint = _endpoint
                    };

                var ocrResult = await client.RecognizePrintedTextInStreamAsync(true, imageStream);

                return ProcessRecognitionResult(ocrResult);

                }
            catch (Exception e)
                {
                return e.Message;
                }
            }

        private static string ProcessRecognitionResult(OcrResult result)
            {
            try
                {
                string recognizedText = "";

                foreach (var region in result.Regions)
                    {
                    foreach (var line in region.Lines)
                        {
                        foreach (var word in line.Words)
                            {
                            recognizedText += word.Text + " ";
                            }
                        recognizedText += "\n";
                        }
                    }

                return recognizedText;

                }
            catch (Exception e)
                {
                return e.Message;
                throw;
                }
            }
        }
    }
