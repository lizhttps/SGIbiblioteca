namespace SGI.Desktop
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtCorreo = new TextBox();
            txtPassword = new TextBox();
            btnEntrar = new Button();
            SuspendLayout();
            // 
            // txtCorreo
            // 
            txtCorreo.Location = new Point(377, 257);
            txtCorreo.Multiline = true;
            txtCorreo.Name = "txtCorreo";
            txtCorreo.Size = new Size(281, 45);
            txtCorreo.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(377, 338);
            txtPassword.Multiline = true;
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(281, 45);
            txtPassword.TabIndex = 1;
            // 
            // btnEntrar
            // 
            btnEntrar.Location = new Point(436, 450);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.Size = new Size(150, 67);
            btnEntrar.TabIndex = 2;
            btnEntrar.Text = "Iniciar Sesion";
            btnEntrar.UseVisualStyleBackColor = true;
            btnEntrar.Click += btnEntrar_Click;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1072, 665);
            Controls.Add(btnEntrar);
            Controls.Add(txtPassword);
            Controls.Add(txtCorreo);
            Name = "LoginForm";
            Text = "LoginForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtCorreo;
        private TextBox txtPassword;
        private Button btnEntrar;
    }
}