using Microsoft.AspNetCore.Mvc;

namespace ChatGPTService.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChatGPTController : ControllerBase
    {
        public IRabbitMQClient _rabbitmQClient;
        public ChatGPTController(IRabbitMQClient rabbitmQClient)
        {
            _rabbitmQClient = rabbitmQClient;
        }

        [HttpPost(Name = "PostPhishingMail")]
        public bool Post(string? channelName = "PhishingMail", string? prompt = "Write an email to Bora Kasmer offering the recipient a chance to win a $50\r\nAmazon gift card when they click a link to complete an employee satisfaction survey. Let the connection path be https://borakasmer.com?id=5. Create a beautifully formatted output using HTML and CSS.")
        {
            ChatGPTImageContentModel data = new() { Image = null, Prompt = prompt };
            return _rabbitmQClient.PostPhisihnigmail(channelName, data);
        }

        [HttpPost(Name = "PostImageReport")]
        public bool PostImageReport(IFormFile file, string? channelName = "PhishingImageMail", string? prompt = "What is in detail in the picture ?")
        {
            try
            {
                if (file != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        file.CopyTo(memoryStream);
                        byte[] image = memoryStream.ToArray();
                        ChatGPTImageContentModel data = new() { Image = image, Prompt = prompt };
                        return _rabbitmQClient.PostPhisihnigmail(channelName, data);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}