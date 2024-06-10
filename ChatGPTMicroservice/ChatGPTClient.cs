using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using static OpenAI.ObjectModels.StaticValues;

//https://github.com/betalgo/openai
namespace ChatGPTMicroservice
{
    public class ChatGPTClient : IDisposable
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public string GetPhishingMail(string prompt)
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "Your-ApiKey"
            });

            const string fileName = "C:\\Projects\\ChatGPT4\\ChatGPTMicroservice\\profile.jpg";
            var binaryImage = System.IO.File.ReadAllBytes(fileName);

            var completionResult = openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a helpful assistant."),
                    ChatMessage.FromUser(prompt),                                   
                },             
                Model = Models.Gpt_4_turbo,
                MaxTokens = 2000,
                FrequencyPenalty = 0,
                Temperature = (float?)0.7,
                PresencePenalty = 0,
            });          

            if (completionResult.Result.Successful)
            {
                return completionResult.Result.Choices.FirstOrDefault().Message.Content;                
            }
            else
            {
                if (completionResult.Result.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                return $"{completionResult.Result.Error.Code}: {completionResult.Result.Error.Message}";
            }
        }
        public string GetPhishingImageMail(ChatGPTImageContentModel chatImageModel)
        {
            var openAiService = new OpenAIService(new OpenAiOptions()
            {
                ApiKey = "Your-ApiKey"
            });            

            var binaryImage = chatImageModel.Image;

            var completionResult = openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest()
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a helpful assistant."),

                    ChatMessage.FromUser(
                        new List<MessageContent>
                        {
                            //MessageContent.TextContent("What is on the picture in details?"),
                            MessageContent.TextContent(chatImageModel.Prompt),
                            MessageContent.ImageBinaryContent(
                                binaryImage,
                                ImageStatics.ImageFileTypes.Png,
                                ImageStatics.ImageDetailTypes.High)
                        }),

                    //ChatMessage.FromUser(
                    //    new List<MessageContent>
                    //    {
                    //        MessageContent.TextContent("What is on the picture in details?"),
                    //        MessageContent.ImageUrlContent(
                    //            "http://www.borakasmer.com/wp-content/uploads/2019/09/profile.jpg", ImageStatics.ImageDetailTypes.High)
                    //    }),
                },              
                Model = Models.Gpt_4_turbo,
                MaxTokens = 500,
                FrequencyPenalty = 0,
                Temperature = (float?)0.7,
                PresencePenalty = 0,
            });            

            if (completionResult.Result.Successful)
            {
                return completionResult.Result.Choices.FirstOrDefault().Message.Content;
                // return completionResult.Result.Choices.FirstOrDefault().Text;
            }
            else
            {
                if (completionResult.Result.Error == null)
                {
                    throw new Exception("Unknown Error");
                }
                return $"{completionResult.Result.Error.Code}: {completionResult.Result.Error.Message}";
            }
        }
    }
}
