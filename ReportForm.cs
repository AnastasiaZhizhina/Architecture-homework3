using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

public class ReportForm : Form
{
    private DataGridView dgvReport1, dgvReport2, dgvReport3;

    public ReportForm()
    {
        InitializeComponent();
        LoadReport();
    }

    private void InitializeComponent()
    {
        dgvReport1 = new DataGridView();
        dgvReport2 = new DataGridView();
        dgvReport3 = new DataGridView();

        Label lbl1 = new Label { Text = "1. Список смартфонов с производителями:", Location = new System.Drawing.Point(10, 10), AutoSize = true };
        Label lbl2 = new Label { Text = "2. Количество смартфонов по производителям:", Location = new System.Drawing.Point(10, 240), AutoSize = true };
        Label lbl3 = new Label { Text = "3. Средняя цена по производителям (убывание):", Location = new System.Drawing.Point(10, 460), AutoSize = true };

        dgvReport1.Location = new System.Drawing.Point(10, 35);
        dgvReport1.Size = new System.Drawing.Size(760, 180);
        dgvReport2.Location = new System.Drawing.Point(10, 265);
        dgvReport2.Size = new System.Drawing.Size(760, 170);
        dgvReport3.Location = new System.Drawing.Point(10, 485);
        dgvReport3.Size = new System.Drawing.Size(760, 170);

        Controls.Add(lbl1);
        Controls.Add(lbl2);
        Controls.Add(lbl3);
        Controls.Add(dgvReport1);
        Controls.Add(dgvReport2);
        Controls.Add(dgvReport3);

        Text = "Отчёт по смартфонам";
        Size = new System.Drawing.Size(800, 700);
        StartPosition = FormStartPosition.CenterScreen;
    }

    private void LoadReport()
    {
        using var ctx = new AppDbContext();

        // Раздел 1 - список смартфонов
        var smartphones = ctx.Smartphones
            .Include(s => s.Manufacturer)
            .OrderBy(s => s.Model)
            .ToList();

        var report1 = smartphones.Select(s => new
        {
            Модель = s.Model,
            Производитель = s.Manufacturer?.Name ?? "",
            Цена = s.Price
        }).ToList();
        dgvReport1.DataSource = report1;

        // Получаем словарь производителей для подстановки имени
        var manufacturers = ctx.Manufacturers.ToDictionary(m => m.Id, m => m.Name);

        // Раздел 2 - количество по категориям
        var report2Raw = ctx.Smartphones
            .GroupBy(s => s.ManufacturerId)
            .Select(g => new
            {
                ManufacturerId = g.Key,
                Count = g.Count()
            })
            .ToList();

        var report2Final = report2Raw.Select(r => new
        {
            Производитель = manufacturers.ContainsKey(r.ManufacturerId) ? manufacturers[r.ManufacturerId] : "Неизвестно",
            Количество = r.Count
        }).OrderBy(r => r.Производитель).ToList();
        dgvReport2.DataSource = report2Final;

        // Раздел 3 - средняя цена по категориям
        var report3Raw = ctx.Smartphones
            .GroupBy(s => s.ManufacturerId)
            .Select(g => new
            {
                ManufacturerId = g.Key,
                AvgPrice = g.Average(s => s.Price)
            })
            .ToList();

        var report3Final = report3Raw.Select(r => new
        {
            Производитель = manufacturers.ContainsKey(r.ManufacturerId) ? manufacturers[r.ManufacturerId] : "Неизвестно",
            СредняяЦена = Math.Round(r.AvgPrice, 2)
        }).OrderByDescending(r => r.СредняяЦена).ToList();
        dgvReport3.DataSource = report3Final;
    }
}