namespace ProyectoMovile.Vistas;

public partial class vMantenimiento : ContentPage
{
	public vMantenimiento()
	{
		InitializeComponent();
	}

    private void btnPerfiles_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Vistas.Perfiles.vPerfil());
    }

    private void btnEntrevistador_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Vistas.Usuarios.vUsuarios());
    }

    private void btnAnalisis_Clicked(object sender, EventArgs e)
    {

    }
}