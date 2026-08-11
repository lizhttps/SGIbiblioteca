namespace SGI.Desktop
{
    static class Program
    {
        public static readonly HttpClient ApiClient = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7194/") // ?? Asegúrate de poner la URL/Puerto de tu API
        };

        [STAThread]
        static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

            // Abrimos el formulario de Login (lo creamos en el Paso 2)
            System.Windows.Forms.Application.Run(new LoginForm());
        }
    }
}
