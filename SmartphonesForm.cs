using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

public partial class SmartphonesForm : Form
{
    public SmartphonesForm()
    {
        InitializeComponent();
        LoadSmartphones();
    }

    private void InitializeComponent()
    {
        dgvSmartphones = new DataGridView();
        btnAdd = new Button { Text = "Добавить", Location = new System.Drawing.Point(20, 300), Size = new System.Drawing.Size(100, 30) };
        btnEdit = new Button { Text = "Редактировать", Location = new System.Drawing.Point(140, 300), Size = new System.Drawing.Size(100, 30) };
        btnDelete = new Button { Text = "Удалить", Location = new System.Drawing.Point(260, 300), Size = new System.Drawing.Size(100, 30) };

        btnAdd.Click += BtnAdd_Click;
        btnEdit.Click += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;

        dgvSmartphones.Dock = DockStyle.Top;
        dgvSmartphones.Height = 250;
        dgvSmartphones.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        Controls.Add(dgvSmartphones);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);

        Text = "Смартфоны";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new System.Drawing.Size(700, 400);
    }

    private DataGridView dgvSmartphones;
    private Button btnAdd, btnEdit, btnDelete;

    private void LoadSmartphones()
    {
        using var context = new AppDbContext();
        var data = context.Smartphones
            .Include(s => s.Manufacturer)
            .Select(s => new
            {
                s.Id,
                Модель = s.Model,
                Производитель = s.Manufacturer != null ? s.Manufacturer.Name : "",
                Цена = s.Price
            })
            .OrderBy(s => s.Модель)
            .ToList();
        dgvSmartphones.DataSource = data;
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        using var context = new AppDbContext();
        var form = new SmartphoneEditForm(context, null);
        if (form.ShowDialog() == DialogResult.OK)
            LoadSmartphones();
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (dgvSmartphones.CurrentRow?.DataBoundItem?.GetType().GetProperty("Id")?.GetValue(dgvSmartphones.CurrentRow.DataBoundItem) is int id)
        {
            using var context = new AppDbContext();
            var smartphone = context.Smartphones.Find(id);
            if (smartphone != null)
            {
                var form = new SmartphoneEditForm(context, smartphone);
                if (form.ShowDialog() == DialogResult.OK)
                    LoadSmartphones();
            }
        }
        else
            MessageBox.Show("Выберите смартфон", "Ошибка");
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (dgvSmartphones.CurrentRow?.DataBoundItem?.GetType().GetProperty("Id")?.GetValue(dgvSmartphones.CurrentRow.DataBoundItem) is int id)
        {
            if (MessageBox.Show("Удалить смартфон?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using var context = new AppDbContext();
                var smartphone = context.Smartphones.Find(id);
                if (smartphone != null)
                {
                    context.Smartphones.Remove(smartphone);
                    context.SaveChanges();
                    LoadSmartphones();
                }
            }
        }
    }
}