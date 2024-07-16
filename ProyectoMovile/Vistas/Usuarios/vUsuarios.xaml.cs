using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;

namespace ProyectoMovile.Vistas.Usuarios;

public partial class vUsuarios : ContentPage
{
    private const string url = "http://localhost:81/Proyecto/selecAnindadoUsuario.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Modelos.Usuarios> users;

    public vUsuarios()
	{
		InitializeComponent();
        obtener();
	}
    public async void obtener()
    {
        var content = await cliente.GetStringAsync(url);
        List<Modelos.Usuarios> mostrarUser = JsonConvert.DeserializeObject<List<Modelos.Usuarios>>(content);
        users = new ObservableCollection<Modelos.Usuarios>(mostrarUser);
        listaUsuarios.ItemsSource = users;

    }

    private async void btnEditar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var users = button.BindingContext as Modelos.Usuarios;
        // Obtener los nuevos valores de entrada del usuario
        string nuevoNombre = await DisplayPromptAsync("Editar", "Ingrese el nuevo nombre:", initialValue: users.usu_nombre);
        string nuevoApellido = await DisplayPromptAsync("Editar", "Ingrese el nuevo apellido:", initialValue: users.usu_apellido);
        string nuevoCorreo = await DisplayPromptAsync("Editar", "Ingrese el nuevo correo:", initialValue: users.usu_correo);
        string nuevaClave = await DisplayPromptAsync("Editar", "Ingrese la nueva clave:", initialValue: users.usu_clave);
        //string nuevoCodPerfil = await DisplayPromptAsync("Editar", "Ingrese el nuevo código de perfil:", initialValue: users.cod_perfil.ToString());
        //string nuevoEstado = await DisplayPromptAsync("Editar", "Ingrese el nuevo estado:", initialValue: users.usu_estado.ToString());

        if (!string.IsNullOrEmpty(nuevoNombre) && !string.IsNullOrEmpty(nuevoApellido) &&
            !string.IsNullOrEmpty(nuevoCorreo) && !string.IsNullOrEmpty(nuevaClave) 
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
                    parametros.Add("usu_cod", users.usu_cod.ToString());
                    parametros.Add("usu_nombre", nuevoNombre);
                    parametros.Add("usu_apellido", nuevoApellido);
                    parametros.Add("usu_correo", nuevoCorreo);
                    parametros.Add("usu_clave", nuevaClave);
                    //parametros.Add("cod_perfil", nuevoCodPerfil);
                    //parametros.Add("per_estado", nuevoEstado);

                    // Crear una URI para la solicitud PUT
                    Uri uri = new Uri($"http://localhost:81/Proyecto/UsuariosEditar.php?per_cod={users.usu_cod}");

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
                    users.usu_nombre = nuevoNombre;
                    users.usu_apellido = nuevoApellido;
                    users.usu_correo = nuevoCorreo;
                    users.usu_clave = nuevaClave;
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

    private async void btnEliinar_Clicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var users = button.BindingContext as Modelos.Usuarios;

        try
        {
            using (HttpClient client = new HttpClient())
            {
                var content = new FormUrlEncodedContent(new[]
                {
                new KeyValuePair<string, string>("per_cod", users.usu_cod.ToString()) // Aseg�rate de tener el ID del estudiante
            });

                var response = await client.DeleteAsync("http://localhost:81/Proyecto/deleteUsuario.php?usu_cod=" + users.usu_cod);
                //var response = await client.DeleteAsync("?codigo=" + txtCodigo.Text);

                if (response.IsSuccessStatusCode)
                {
                    DisplayAlert("alerta", "Usuario eliminado correctamente", "cerrar");

                    await Navigation.PushAsync(new vUsuarios());
                }
                else
                {
                    DisplayAlert("alerta", "Error al eliminar Usuario", "cerrar");
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

            parametros.Add("usu_nombre", txtNombre.Text);
            parametros.Add("usu_apellido", txtApellido.Text);
            parametros.Add("usu_correo", txtEmail.Text);
            parametros.Add("usu_clave", txtClave.Text);
            parametros.Add("cod_perfil", txtPerfil.Text);
            parametros.Add("usu_estado", txtEstado.Text);

            cliente.UploadValues("http://localhost:81/Proyecto/PostUsuarios.php", "post", parametros);

            Navigation.PushAsync(new vUsuarios());


        }
        catch (Exception ex)
        {

            DisplayAlert("alerta", ex.Message, "cerrar");
        }
    }
}