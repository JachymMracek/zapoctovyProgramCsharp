namespace zapoctovyProgramFinal
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "herna";

            this.BackgroundImage = System.Drawing.Image.FromFile(@"C:\Users\Admin\Documents\ZAPOCTOVY_PROGRAM_Csharp\obrazky1\backGround.jpg");

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
