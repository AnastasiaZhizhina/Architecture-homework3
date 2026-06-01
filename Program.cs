using System;
using System.Windows.Forms;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        using var context = new AppDbContext();
        context.InitializeDatabase();

        Application.Run(new MainForm());
    }
}