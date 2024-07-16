using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace ProyectoMovile.Vistas.Cliente;

public partial class vCliente : ContentPage
{
    private const string url = "http://localhost:81/Proyecto/selectCliente.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Modelos.Cliente> clientes;
    public vCliente()
	{
		InitializeComponent();
        obtener();
	}
    public async void obtener()
    {
        var content = await cliente.GetStringAsync(url);
        List<Modelos.Cliente> mostrarCli = JsonConvert.DeserializeObject<List<Modelos.Cliente>>(content);
        clientes = new ObservableCollection<Modelos.Cliente>(mostrarCli);
        listaClientes.ItemsSource = clientes;

    }

    private async void  btnEditar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var clientes = button.BindingContext as Modelos.Cliente;
        // Obtener los nuevos valores de entrada del usuario
        string nuevoNombre = await DisplayPromptAsync("Editar", "Ingrese el nuevo nombre:", initialValue: clientes.cli_nombre);
        string nuevoApellido = await DisplayPromptAsync("Editar", "Ingrese el nuevo apellido:", initialValue: clientes.cli_apellido);
        //string nuevoFN = await DisplayPromptAsync("Editar", "Ingrese la nueva Fecha nacimiento:", initialValue: clientes.cli_fechaNac);
        string nuevaEdad = await DisplayPromptAsync("Editar", "Ingrese la nueva edad:", initialValue: clientes.cli_edad.ToString());

        if (!string.IsNullOrEmpty(nuevoNombre) && !string.IsNullOrEmpty(nuevoApellido) 
            //&&
            //!string.IsNullOrEmpty(nuevoFN)
            && !string.IsNullOrEmpty(nuevaEdad)
            //&&
            //!string.IsNullOrEmpty(nuevoCodPerfil) &&
            //!string.IsNullOrEmpty(nuevoEstado)
            )
        {
            try
            {
                // Código para actualizar en el servidor web usando PUT
                using (WebClient cliente = new WebClient())
                {
                    var parametros = new System.Collections.Specialized.NameValueCollection();
                    parametros.Add("cli_cod", clientes.cli_cod.ToString());
                    parametros.Add("cli_nombre", nuevoNombre);
                    parametros.Add("cli_apellido", nuevoApellido);
                    //parametros.Add("cli_fechaNac", nuevoFN);
                    parametros.Add("cli_edad", nuevaEdad);

                    // Crear una URI para la solicitud PUT
                    Uri uri = new Uri($"http://localhost:81/Proyecto/ClienteEditar.php?cli_cod={clientes.cli_cod}");

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

                    // Actualiza los valores en el objeto localmente
                    clientes.cli_nombre = nuevoNombre;
                    clientes.cli_apellido = nuevoApellido;

                    //clientes.cli_fechaNac = nuevoFN;
                    clientes.cli_edad = int.Parse(nuevaEdad);
                    //users.cod_perfil = int.Parse(nuevoCodPerfil);
                    //users.usu_estado = int.Parse(nuevoEstado);

                    await Navigation.PushAsync(new Vistas.Cliente.vCliente());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo actualizar en el servidor: {ex.Message}", "OK");
            }

        }
    }

        private async void btnEliinar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var clientes = button.BindingContext as Modelos.Cliente;

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("cli_cod", clientes.cli_cod.ToString()) // Aseg�rate de tener el ID del estudiante
            });

                var response = await client.DeleteAsync("http://localhost:81/Proyecto/deleteCliente.php?cli_cod=" + clientes.cli_cod);
                
                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("alerta", "Cliente eliminado correctamente", "cerrar");

                    await Navigation.PushAsync(new vCliente());
                }
                else
                {
                    DisplayAlert("alerta", "Error al eliminar Cliente", "cerrar");
                }
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("alerta", ex.Message, "cerrar");
        }
    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {
        try
        {
            WebClient cliente = new WebClient();

            var parametros = new System.Collections.Specialized.NameValueCollection();

            parametros.Add("cli_ci", txtDNI.Text);
            parametros.Add("cli_nombre", txtNombre.Text);
            parametros.Add("cli_apellido", txtApellido.Text);
            parametros.Add("cli_fechaNac", txtFN.Text);
            parametros.Add("cli_edad", txtEdad.Text);
            parametros.Add("cli_estado", txtEstado.Text);

            cliente.UploadValues("http://localhost:81/Proyecto/CLientePost.php", "post", parametros);

            Navigation.PushAsync(new vCliente());


        }
        catch (Exception ex)
        {

            DisplayAlert("alerta", ex.Message, "cerrar");
        }
    }
}