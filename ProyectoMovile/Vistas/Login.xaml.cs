using Microsoft.Win32;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace ProyectoMovile.Vistas;

public partial class Login : ContentPage
{
   
    public Login()
	{
		InitializeComponent();
       
	}
   

    private async void btnAutenticar_Clicked(object sender, EventArgs e)
    {
        string connectionString = "Server=localhost;Database=proyectodm;User ID=root;Password=;";

        string usuario = txtUser.Text;
        string clave = txtPassword.Text;

        bool autenticacion = false;

        using (MySqlConnection connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM tbl_usuario WHERE usu_correo = @usuario AND usu_clave = @clave";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@usuario", usuario);
                command.Parameters.AddWithValue("@clave", clave);

                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count > 0)
                {
                    autenticacion = true;
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
                DisplayAlert("Error", "Se ha producido un error: " + ex.Message, "cerrar");
            }
        }

        if (autenticacion)
        {
            DisplayAlert("Autenticación", "Inicio de sesión correcto", "ok");
            Navigation.PushAsync(new Vistas.vMenu());
        }
        else
        {
            DisplayAlert("Alerta", "Error en el usuario o contraseña", "cerrar");
        }
    }
    


   
}




