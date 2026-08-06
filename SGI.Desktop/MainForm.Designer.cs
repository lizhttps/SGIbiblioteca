namespace SGI.Desktop
{
    partial class MainForm
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
            lblBienvenida = new Label();
            dgvLibros = new DataGridView();
            btnCargarLibros = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLibros).BeginInit();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            lblBienvenida.Location = new Point(377, 29);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(384, 46);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Panel del Bibliotecario";
            // 
            // dgvLibros
            // 
            dgvLibros.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLibros.Location = new Point(163, 120);
            dgvLibros.Name = "dgvLibros";
            dgvLibros.RowHeadersWidth = 51;
            dgvLibros.Size = new Size(876, 440);
            dgvLibros.TabIndex = 1;
            // 
            // btnCargarLibros
            // 
            btnCargarLibros.Location = new Point(535, 585);
            btnCargarLibros.Name = "btnCargarLibros";
            btnCargarLibros.Size = new Size(94, 29);
            btnCargarLibros.TabIndex = 2;
            btnCargarLibros.Text = "button1";
            btnCargarLibros.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1180, 626);
            Controls.Add(btnCargarLibros);
            Controls.Add(dgvLibros);
            Controls.Add(lblBienvenida);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)dgvLibros).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
        private DataGridView dgvLibros;
        private Button btnCargarLibros;
    }
}