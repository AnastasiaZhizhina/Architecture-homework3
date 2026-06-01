using System;
using System.Linq;
using System.Windows.Forms;

public partial class SmartphoneEditForm : Form
{
    private readonly AppDbContext _context;
    private readonly Smartphone? _smartphone;
    private TextBox txtModel;
    private NumericUpDown numPrice;
    private ComboBox cboManufacturer;
    private Button btnSave;

    public SmartphoneEditForm(AppDbContext context, Smartphone? smartphone = null)
    {
        _context = context;
        _smartphone = smartphone;
        InitializeComponent();
        LoadManufacturers();
        if (_smartphone != null)
        {
            txtModel.Text = _smartphone.Model;
            numPrice.Value = _smartphone.Price;
            cboManufacturer.SelectedValue = _smartphone.ManufacturerId;
        }
    }

    private void InitializeComponent()
    {
        txtModel = new TextBox { Location = new System.Drawing.Point(120, 20), Width = 200 };
        numPrice = new NumericUpDown { Location = new System.Drawing.Point(120, 60), Width = 200, Minimum = 0, Maximum = 10000000 };
        cboManufacturer = new ComboBox { Location = new System.Drawing.Point(120, 100), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        btnSave = new Button { Text = "Сохранить", Location = new System.Drawing.Point(120, 140), Width = 100 };

        Label lblModel = new Label { Text = "Модель:", Location = new System.Drawing.Point(20, 22) };
        Label lblPrice = new Label { Text = "Цена (руб):", Location = new System.Drawing.Point(20, 62) };
        Label lblMan = new Label { Text = "Производитель:", Location = new System.Drawing.Point(20, 102) };

        Controls.Add(lblModel);
        Controls.Add(lblPrice);
        Controls.Add(lblMan);
        Controls.Add(txtModel);
        Controls.Add(numPrice);
        Controls.Add(cboManufacturer);
        Controls.Add(btnSave);

        btnSave.Click += BtnSave_Click;

        Text = _smartphone == null ? "Добавление смартфона" : "Редактирование смартфона";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new System.Drawing.Size(380, 240);
    }

    private void LoadManufacturers()
    {
        cboManufacturer.DataSource = _context.Manufacturers.OrderBy(m => m.Name).ToList();
        cboManufacturer.DisplayMember = "Name";
        cboManufacturer.ValueMember = "Id";
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtModel.Text))
        {
            MessageBox.Show("Введите модель", "Ошибка");
            return;
        }

        if (numPrice.Value < 0)
        {
            MessageBox.Show("Цена не может быть отрицательной", "Ошибка");
            return;
        }

        if (cboManufacturer.SelectedValue == null)
        {
            MessageBox.Show("Выберите производителя", "Ошибка");
            return;
        }

        if (_smartphone == null)
        {
            var newPhone = new Smartphone
            {
                Model = txtModel.Text,
                Price = numPrice.Value,
                ManufacturerId = (int)cboManufacturer.SelectedValue
            };
            _context.Smartphones.Add(newPhone);
        }
        else
        {
            _smartphone.Model = txtModel.Text;
            _smartphone.Price = numPrice.Value;
            _smartphone.ManufacturerId = (int)cboManufacturer.SelectedValue;
            _context.Smartphones.Update(_smartphone);
        }

        _context.SaveChanges();
        DialogResult = DialogResult.OK;
        Close();
    }
}