using StravaGpxConverter.Services.Interfaces;

namespace StravaGpxConverter.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }
    }
}
