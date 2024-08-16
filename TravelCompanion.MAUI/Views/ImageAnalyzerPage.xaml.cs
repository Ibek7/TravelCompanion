using Microsoft.Maui.Controls;
using Microsoft.Maui.Media;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpenAI.Chat;

namespace TravelCompanion.MAUI.Views
{
    public partial class ImageAnalyzerPage : ContentPage
    {
        private readonly ChatClient _chatClient;
        private readonly IMediaPicker _mediaPicker;
        private byte[] _imageBytes;
        private string _contentType;

        public ImageAnalyzerPage(ChatClient chatClient, IMediaPicker mediaPicker)
        {
            InitializeComponent();
            _chatClient = chatClient;
            _mediaPicker = mediaPicker;
        }

        private async void OnPickImageButtonClicked(object sender, EventArgs e)
        {
            var photo = await _mediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Pick a photo"
            });

            if (photo != null)
            {
                using var stream = await photo.OpenReadAsync();

                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    _imageBytes = memoryStream.ToArray();
                }

                _contentType = photo.ContentType;

                // Set the Image control's source
                SelectedImage.Source = ImageSource.FromStream(() => new MemoryStream(_imageBytes));

                // You now have the image bytes in the imageBytes variable, and you can use them as needed.
            }
        }


        private async void OnAnalyzeImageButtonClicked(object sender, EventArgs e)
        {
            AnalysisResultLabel.Text = string.Empty;

            if (SelectedImage.Source == null)
            {
                await DisplayAlert("Error", "Please select an image first.", "OK");
                return;
            }

            // Check if the image is too small
            if (_imageBytes == null || _imageBytes.Length < 1) // Adjust the size limit as needed
            {
                await DisplayAlert("Error", "The selected image is too small to be processed.", "OK");
                return;
            }

            var messages = new ChatMessage[]
            {
                new SystemChatMessage("Analyze the content of the image provided."),
                new UserChatMessage(
                    ChatMessageContentPart.CreateTextMessageContentPart("Can you tell me about this picture?"),
                    ChatMessageContentPart.
                    CreateImageMessageContentPart(new BinaryData(_imageBytes), _contentType))
            };

            // Send the image content with the chat messages for analysis.
            var streamingResult = _chatClient.CompleteChatStreamingAsync(messages);

            await foreach (StreamingChatCompletionUpdate chatUpdate in streamingResult)
            {
                foreach (var updatePart in chatUpdate.ContentUpdate)
                {
                    //Concatenate the new part of the response with the existing response
                    AnalysisResultLabel.Text += updatePart.Text;
                }
            }
        }
    }
}