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
        string nuevoFN = await DisplayPromptAsync("Editar", "Ingrese el nuevo fecha Nacimiento:", initialValue: clientes.cli_fechaNac);
        string nuevaEdad = await DisplayPromptAsync("Editar", "Ingrese la nueva edad:", initialValue: clientes.cli_edad.ToString());
        //string nuevoCodPerfil = await DisplayPromptAsync("Editar", "Ingrese el nuevo código de perfil:", initialValue: users.cod_perfil.ToString());
        //string nuevoEstado = await DisplayPromptAsync("Editar", "Ingrese el nuevo estado:", initialValue: users.usu_estado.ToString());

        if (!string.IsNullOrEmpty(nuevoNombre) && !string.IsNullOrEmpty(nuevoApellido) &&
            !string.IsNullOrEmpty(nuevoFN) && !string.IsNullOrEmpty(nuevaEdad)
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
                    //parametros.Add("usu_cod", clientes.cli_cod.ToString());
                    parametros.Add("cli_nombre", nuevoNombre);
                    parametros.Add("cli_apellido", nuevoApellido);
                    parametros.Add("cli_fechaNac", nuevoFN);
                    parametros.Add("cli_edad", nuevaEdad);
                    //parametros.Add("cod_perfil", nuevoCodPerfil);
                    //parametros.Add("per_estado", nuevoEstado);

                    // Crear una URI para la solicitud PUT
                    Uri uri = new Uri($"http://localhost:81/Proyecto/UsuariosEditar.php?per_cod={clientes.cli_cod}");

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
                    clientes.cli_fechaNac = nuevoFN;
                    clientes.cli_edad = int.Parse(nuevaEdad);
                    //users.cod_perfil = int.Parse(nuevoCodPerfil);
                    //users.usu_estado = int.Parse(nuevoEstado);

                    await Navigation.PushAsync(new Vistas.Usuarios.vUsuarios());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo actualizar en el servidor: {ex.Message}", "OK");
            }
        }

    }

    private void btnEliinar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnAgregar_Clicked(object sender, EventArgs e)
    {

    }
}