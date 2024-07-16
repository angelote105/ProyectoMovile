using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace ProyectoMovile.Vistas;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private  void btnAutenticar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new vMenu());
    }
   
    
    private  void btnValdiarBD_Clicked(object sender, EventArgs e)
    {
    } 
}




