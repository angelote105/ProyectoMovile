using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace ProyectoMovile.Vistas.Perfiles;

public partial class vPerfil : ContentPage
{
    private const string url = "http://localhost:81/Proyecto/selectPerfil.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Modelos.Perfiles> per;
    public vPerfil()
	{
		InitializeComponent();
        obtener();
	}
    public async void obtener()
    {
        var content = await cliente.GetStringAsync(url);
        List<Modelos.Perfiles> mostrarPer = JsonConvert.DeserializeObject<List<Modelos.Perfiles>>(content);
        per = new ObservableCollection<Modelos.Perfiles>(mostrarPer);
        listaPerfiles.ItemsSource = per;

    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();

            var parametros = new System.Collections.Specialized.NameValueCollection();

            parametros.Add("per_nombre", txtNombre.Text);

            cliente.UploadValues("http://localhost:81/Proyecto/PostPerfil.php", "post", parametros);

            Navigation.PushAsync(new vPerfil());


        }
        catch (Exception ex)
        {

            DisplayAlert("alerta", ex.Message, "cerrar");
        }
    }

    private async void btnEditar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var perfill = button.BindingContext as Modelos.Perfiles;



        if (perfill != null)
        {
            string nuevoNombre = await DisplayPromptAsync("Editar", "Ingrese el nuevo nombre:", initialValue: perfill.per_nombre);
            if (!string.IsNullOrEmpty(nuevoNombre))
            {
                try
                {
                    // Código para actualizar en el servidor web usando PUT
                    using (WebClient cliente = new WebClient())
                    {
                        var parametros = new System.Collections.Specialized.NameValueCollection();
                        parametros.Add("per_nombre", nuevoNombre);
                        parametros.Add("per_cod", perfill.per_cod.ToString()); // Asegúrate de enviar también el ID de la persona

                        // Crear una URI para la solicitud PUT
                        Uri uri = new Uri($"http://localhost:81/Proyecto/perfil.php?per_cod={perfill.per_cod}");

                        // Convertir los parámetros en una cadena de consulta
                        string queryString = string.Join("&", parametros.AllKeys.Select(key => $"{key}={parametros[key]}"));

                        // Crear un arreglo de bytes para los datos del PUT
                        byte[] data = Encoding.UTF8.GetBytes(queryString);

                        cliente.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                        // Realizar la solicitud PUT
                        byte[] response = cliente.UploadData(uri, "PUT", data);

                        // Opcionalmente, podrías manejar la respuesta del servidor aquí
                        string responseString = Encoding.UTF8.GetString(response);
                        await DisplayAlert("Servidor", $"Respuesta del servidor: {responseString}", "OK");

                        // Actualiza el nombre en el objeto localmente
                        perfill.per_nombre = nuevoNombre;

                        Navigation.PushAsync(new Vistas.Perfiles.vPerfil());
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"No se pudo actualizar en el servidor: {ex.Message}", "OK");
                }
            }
        }

    }

    private async void btnEliinar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var perfill = button.BindingContext as Modelos.Perfiles;

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("per_cod", perfill.per_cod.ToString()) // Aseg�rate de tener el ID del estudiante
            });

                var response = await client.DeleteAsync("http://localhost:81/Proyecto/deletePerfil.php?per_cod=" + perfill.per_cod);
                //var response = await client.DeleteAsync("?codigo=" + txtCodigo.Text);

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("alerta", "Estudiante eliminado correctamente", "cerrar");

                    await Navigation.PushAsync(new vPerfil());
                }
                else
                {
                    DisplayAlert("alerta", "Error al eliminar el estudiante", "cerrar");
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("alerta", ex.Message, "cerrar");
        }
    }
}