namespace ProyectoMovile.Vistas;

public partial class Login : ContentPage
{
	public Login()
	{
		InitializeComponent();
	}

    private void btnAutenticar_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Vistas.vMenu());
    }
}