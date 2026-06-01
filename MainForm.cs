using System;
using System.Windows.Forms;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        Text = "Учёт смартфонов";
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void InitializeComponent()
    {
        btnManufacturers = new Button { Text = "Производители", Location = new System.Drawing.Point(50, 50), Size = new System.Drawing.Size(200, 50) };
        btnSmartphones = new Button { Text = "Смартфоны", Location = new System.Drawing.Point(50, 120), Size = new System.Drawing.Size(200, 50) };
        btnReport = new Button { Text = "Отчёт", Location = new System.Drawing.Point(50, 190), Size = new System.Drawing.Size(200, 50) };

        btnManufacturers.Click += (s, e) => { new ManufacturersForm().ShowDialog(); };
        btnSmartphones.Click += (s, e) => { new SmartphonesForm().ShowDialog(); };
        btnReport.Click += (s, e) => { new ReportForm().ShowDialog(); };

        Controls.Add(btnManufacturers);
        Controls.Add(btnSmartphones);
        Controls.Add(btnReport);
    }

    private Button btnManufacturers, btnSmartphones, btnReport;
}