using StravaGpxConverter.Services.Interfaces;
using System.Windows;

namespace StravaGpxConverter.Services
{
    public class MessageService : IMessageService
    {
        public string GetMessage()
        {
            return "Hello from the Message Service";
        }

        public MessageBoxResult Question(string message)
        {
            return MessageBox.Show(message,
                "問合わせ",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question);
        }

        public void ShowDialog(string message)
        {
            MessageBox.Show(message);
        }

        public string ShowFileDialog()
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "gpx files (*.gpx)|*.gpx|gpx backup files (*.gpx_bak*)|*.gpx_bak";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return openFileDialog.FileName;
                }
            }

            return string.Empty;
        }
    }
}
