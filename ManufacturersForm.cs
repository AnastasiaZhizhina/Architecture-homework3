using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows.Forms;

public partial class ManufacturersForm : Form
{
    public ManufacturersForm()
    {
        InitializeComponent();
        LoadManufacturers();
    }

    private void InitializeComponent()
    {
        dgvManufacturers = new DataGridView();
        btnAdd = new Button { Text = "Добавить", Location = new System.Drawing.Point(20, 300), Size = new System.Drawing.Size(100, 30) };
        btnEdit = new Button { Text = "Редактировать", Location = new System.Drawing.Point(140, 300), Size = new System.Drawing.Size(100, 30) };
        btnDelete = new Button { Text = "Удалить", Location = new System.Drawing.Point(260, 300), Size = new System.Drawing.Size(100, 30) };

        btnAdd.Click += BtnAdd_Click;
        btnEdit.Click += BtnEdit_Click;
        btnDelete.Click += BtnDelete_Click;

        dgvManufacturers.Dock = DockStyle.Top;
        dgvManufacturers.Height = 250;
        dgvManufacturers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        Controls.Add(dgvManufacturers);
        Controls.Add(btnAdd);
        Controls.Add(btnEdit);
        Controls.Add(btnDelete);

        Text = "Производители";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new System.Drawing.Size(500, 400);
    }

    private DataGridView dgvManufacturers;
    private Button btnAdd, btnEdit, btnDelete;

    private void LoadManufacturers()
    {
        using var context = new AppDbContext();
        dgvManufacturers.DataSource = context.Manufacturers.OrderBy(m => m.Name).ToList();
        dgvManufacturers.Columns["Smartphones"].Visible = false;
    }

    private void BtnAdd_Click(object? sender, EventArgs e)
    {
        string name = Microsoft.VisualBasic.Interaction.InputBox("Введите название производителя", "Добавление");
        if (!string.IsNullOrWhiteSpace(name))
        {
            using var context = new AppDbContext();
            context.Manufacturers.Add(new Manufacturer { Name = name });
            context.SaveChanges();
            LoadManufacturers();
        }
    }

    private void BtnEdit_Click(object? sender, EventArgs e)
    {
        if (dgvManufacturers.CurrentRow?.DataBoundItem is Manufacturer m)
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Редактирование названия", "Изменить", m.Name);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                using var context = new AppDbContext();
                var man = context.Manufacturers.Find(m.Id);
                if (man != null)
                {
                    man.Name = newName;
                    context.SaveChanges();
                    LoadManufacturers();
                }
            }
        }
        else
            MessageBox.Show("Выберите производителя", "Ошибка");
    }

    private void BtnDelete_Click(object? sender, EventArgs e)
    {
        if (dgvManufacturers.CurrentRow?.DataBoundItem is Manufacturer m)
        {
            using var context = new AppDbContext();
            var man = context.Manufacturers.Include(x => x.Smartphones).FirstOrDefault(x => x.Id == m.Id);
            if (man != null && man.Smartphones.Any())
            {
                MessageBox.Show("Нельзя удалить производителя, у которого есть смартфоны!", "Запрещено");
                return;
            }

            if (MessageBox.Show("Удалить?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                context.Manufacturers.Remove(man!);
                context.SaveChanges();
                LoadManufacturers();
            }
        }
    }
}