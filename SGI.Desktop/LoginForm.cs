
using SGI.Application.Dtos.Auth;
using SGI.Shared.Services.Auth;
using SGI.Shared.Session;

namespace SGI.Desktop
{
    public partial class LoginForm : Form
    {
        private readonly IAuthApiService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthApiService(Program.ApiClient);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private async void btnEntrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCorreo.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Ingresá correo y contraseña.", "Datos incompletos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnEntrar.Enabled = false;

            var dto = new UsuarioLoginDto
            {
                Correo = txtCorreo.Text.Trim(),
                Password = txtPassword.Text
            };

            var result = await _authService.LoginAsync(dto);

            if (result.Success && result.Data != null)
            {
                UserSession.IniciarSesion(result.Data);

                var main = new MainForm();
                main.FormClosed += (s, args) => System.Windows.Forms.Application.Exit();
                main.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show(result.Message ?? "Credenciales inválidas.", "Error de acceso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            btnEntrar.Enabled = true;
        }
    }
}