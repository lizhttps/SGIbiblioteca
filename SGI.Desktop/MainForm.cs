using SGI.WEB.Services.Libro;
namespace SGI.Desktop;


public partial class MainForm : Form
{
    private readonly ILibroApiService _libroApiService;

    public MainForm()
    {
        InitializeComponent();
        // Inyectamos el servicio que ya tienes en SGI.Shared
        _libroApiService = new LibroApiService(Program.ApiClient);
    }

    // Evento que se ejecuta al presionar el botón "Cargar Libros"
    private async void btnCargarLibros_Click(object sender, EventArgs e)
    {
        try
        {
            btnCargarLibros.Enabled = false;
            btnCargarLibros.Text = "Cargando...";

            // 1. Llamamos a la API usando tu servicio
            var response = await _libroApiService.GetLibros(); // O GetData() según tu interfaz

            if (response != null && response.Success && response.Data != null)
            {
                // 2. ¡EL TRUCO MÁGICO! Le pasamos la lista directamente a la tabla
                dgvLibros.DataSource = null;
                dgvLibros.DataSource = response.Data;
            }
            else
            {
                MessageBox.Show("No se pudieron obtener los libros.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error de conexión: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnCargarLibros.Enabled = true;
            btnCargarLibros.Text = "📚 Cargar Libros";
        }
    }
}
