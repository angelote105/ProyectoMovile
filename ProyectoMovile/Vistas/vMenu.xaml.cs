namespace ProyectoMovile.Vistas;

public partial class vMenu : ContentPage
{
	public vMenu()
	{
		InitializeComponent();
	}

    private void btnRealizarEntrevista_Clicked(object sender, EventArgs e)
    {

    }

    private void btnRegistroCliente_Clicked(object sender, EventArgs e)
    {

    }

    private void btnMantenimiento_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Vistas.vMantenimiento());
    }

    private void btnSalir_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Vistas.Login());
    }
}