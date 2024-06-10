// See https://aka.ms/new-console-template for more information

using ChatGPTMicroservice;
using System.Text.Json;

using (RabbitMQClient rabbitMQClient = new())
using (ChatGPTClient chatGPTClient = new())
{

    //Prompt for ChatGPT
    var consumer = rabbitMQClient.GetConsumer("PhishingMail");
    if (consumer != null)
    {
        consumer.Received += (model, ea) =>
        {
            var tempData = ea.Body.ToArray();
            var data = System.Text.Encoding.UTF8.GetString(tempData);

            var chatImageModel = JsonSerializer.Deserialize<ChatGPTImageContentModel>(data);
            var phishingMail = chatGPTClient.GetPhishingMail(chatImageModel.Prompt);
            Console.WriteLine(phishingMail);
        };
    }
    //Image for ChatGPT
    var consumer2 = rabbitMQClient.GetConsumer("PhishingImageMail");
    if (consumer2 != null)
    {
        consumer2.Received += (model, ea) =>
        {
            var tempData = ea.Body.ToArray();
            var data = System.Text.Encoding.UTF8.GetString(tempData);

            var chatImageModel = JsonSerializer.Deserialize<ChatGPTImageContentModel>(data);
            var phishingMail = chatGPTClient.GetPhishingImageMail(chatImageModel);
            Console.WriteLine(phishingMail);
        };
    }
    Console.WriteLine("press any key to Exit");
    Console.ReadLine();
}
