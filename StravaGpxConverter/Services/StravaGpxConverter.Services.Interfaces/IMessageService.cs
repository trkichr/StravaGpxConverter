
using System.Collections.Generic;

namespace StravaGpxConverter.Services.Interfaces
{
    public interface IMessageService
    {
        string GetMessage();
        void ShowDialog(string message);
        string[] ShowFileDialog();
    }
}
