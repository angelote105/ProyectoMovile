using Microsoft.Maui.Controls.Internals;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace ProyectoMovile.Vistas.Analisis;

public partial class vAnalisis : ContentPage
{
    private const string url = "http://localhost:81/Proyecto/selectAnalisis.php";
    private readonly HttpClient cliente = new HttpClient();
    private ObservableCollection<Modelos.Analisis> resultados;
    public vAnalisis()
    {
        InitializeComponent();
        obtener();
    } 

    public async void obtener()
    {
        var content = await cliente.GetStringAsync(url);
        List<Modelos.Analisis> mostrarResultados = JsonConvert.DeserializeObject<List<Modelos.Analisis>>(content);
        resultados = new ObservableCollection<Modelos.Analisis>(mostrarResultados);
        listaResultados.ItemsSource = resultados;

    }

    private void btnEditar_Clicked(object sender, EventArgs e)
    {

    }

    private void btnEliinar_Clicked(object sender, EventArgs e)
    {

    }

    
}