using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace ProyectoMovile.Vistas.Negocio;

public partial class ImageCatcher : ContentPage
{
	public ImageCatcher()
	{
		InitializeComponent();
	}

    private async void btnStar_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Solicita permisos en tiempo de ejecuci�n
            var cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
            var storageStatus = await Permissions.RequestAsync<Permissions.StorageRead>();

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Permiso denegado", "No se otorgaron los permisos necesarios para acceder a la cámara.", "OK");
                return;
            }

            // Captura una foto
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
            {
                StatusLabel.Text = "No se capturó ninguna foto.";
                return;
            }

            StatusLabel.Text = "Foto capturada exitosamente.";

            // Abre un stream de la foto
            var stream = await photo.OpenReadAsync();
            var emotionResult = await SendFrameToApi(stream);

            if (emotionResult != null)
            {
                if (!string.IsNullOrEmpty(emotionResult.DominantEmotion))
                {
                    EmotionLabel.Text = $"Emoción: {emotionResult.DominantEmotion}";
                    EmotionLabel.TextColor = Color.FromRgb(emotionResult.Color[2], emotionResult.Color[1], emotionResult.Color[0]);
                    StatusLabel.Text = "Emoción detectada exitosamente.";
                }
                else
                {
                    StatusLabel.Text = "No se detectó ninguna emoción.";
                }
            }
            else
            {
                StatusLabel.Text = "Error al enviar los datos.";
            }

        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
        }

    }

    private async Task<EmotionResult> SendFrameToApi(Stream stream)
    {
        var httpClient = new HttpClient();
        using (var memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            var imageBytes = memoryStream.ToArray();
            var base64Image = Convert.ToBase64String(imageBytes);

            var json = JsonConvert.SerializeObject(new { image = base64Image });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://127.0.0.1:5000/detect_emotions", content);


            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                //return responseContent; // Devuelve la respuesta del servidor como un string
                var result = JsonConvert.DeserializeObject<EmotionResult[]>(responseContent);

                if (result != null && result.Length > 0)
                {
                    Debug.WriteLine($"Emoción dominante: {result[0].DominantEmotion}");
                    return result[0]; // Devuelve el primer resultado
                }
                else
                {
                    Debug.WriteLine("No se detectaron emociones.");
                    return null;
                }
                // Devuelve el primer resultado
            }
            else
            {
                Debug.WriteLine($"Error en la respuesta: {response.StatusCode}");
                return null;
            }
        }
    }

    public class EmotionResult
    {
        [JsonProperty("dominant_emotion")]
        public string DominantEmotion { get; set; }
        [JsonProperty("box")]
        public int[] Box { get; set; }
        [JsonProperty("color")]
        public int[] Color { get; set; }
    }

    private void btnSave_Clicked(object sender, EventArgs e)
    {

    }
}