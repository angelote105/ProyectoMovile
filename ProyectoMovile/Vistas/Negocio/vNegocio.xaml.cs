using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ProyectoMovile.Vistas.Negocio;

public partial class vNegocio : ContentPage
{

    private const string url = "http://localhost:81/Proyecto/selectCliente.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Modelos.Cliente> clientes;
    public vNegocio()
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
}