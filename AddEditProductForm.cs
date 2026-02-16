using System;
using System.Windows.Forms;

namespace InternetShop
{
    public partial class AddEditProductForm : Form
    {
        // Свойства для доступа к данным из MainForm
        public string ProductName => txtName.Text.Trim();
        public double Price => (double)numPrice.Value;
        public bool HasDiscount => chkDiscount.Checked;
        public double DiscountPercent => (double)numDiscountPercent.Value;

        private bool isEditMode = false;

        public AddEditProductForm()
        {
            InitializeComponent();
            ConfigureForAdd();
        }

        public AddEditProductForm(bool isEditMode) : this()
        {
            this.isEditMode = isEditMode;
            ConfigureForEdit();
        }

        private void ConfigureForAdd()
        {
            Text = "Добавить новый товар";
            btnSave.Text = "Добавить";
            numDiscountPercent.Enabled = false;
        }

        private void ConfigureForEdit()
        {
            Text = "Редактировать товар";
            btnSave.Text = "Сохранить";
        }

        public void SetDiscountMode(bool withDiscount)
        {
            chkDiscount.Checked = withDiscount;
            if (withDiscount)
            {
                numDiscountPercent.Value = 10;
            }
        }

        public void SetData(string name, double price, bool hasDiscount, double discountPercent)
        {
            txtName.Text = name;
            numPrice.Value = (decimal)price;
            chkDiscount.Checked = hasDiscount;
            numDiscountPercent.Value = (decimal)discountPercent;
            numDiscountPercent.Enabled = hasDiscount;
            UpdateFinalPrice();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(ProductName))
            {
                MessageBox.Show("Введите название товара", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (ProductName.Length > 100)
            {
                MessageBox.Show("Название товара не может превышать 100 символов", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Price <= 0)
            {
                MessageBox.Show("Цена должна быть больше 0", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                numPrice.Focus();
                return false;
            }

            if (Price > 1000000)
            {
                MessageBox.Show("Цена не может превышать 1 000 000", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                numPrice.Focus();
                return false;
            }

            if (HasDiscount)
            {
                if (DiscountPercent < 0 || DiscountPercent > 100)
                {
                    MessageBox.Show("Скидка должна быть от 0 до 100%", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    numDiscountPercent.Focus();
                    return false;
                }
            }

            return true;
        }

        private void chkDiscount_CheckedChanged(object sender, EventArgs e)
        {
            numDiscountPercent.Enabled = chkDiscount.Checked;

            if (!chkDiscount.Checked)
                numDiscountPercent.Value = 0;

            UpdateFinalPrice();
        }

        private void numPrice_ValueChanged(object sender, EventArgs e)
        {
            UpdateFinalPrice();
        }

        private void numDiscountPercent_ValueChanged(object sender, EventArgs e)
        {
            UpdateFinalPrice();
        }

        private void UpdateFinalPrice()
        {
            try
            {
                double basePrice = (double)numPrice.Value;
                double discountPercent = chkDiscount.Checked ? (double)numDiscountPercent.Value : 0;
                double finalPrice = basePrice * (1 - discountPercent / 100.0);

                lblFinalPrice.Text = $"Итоговая цена: {finalPrice:C2}";

                if (chkDiscount.Checked && discountPercent > 0)
                {
                    lblDiscountAmount.Text = $"Экономия: {(basePrice - finalPrice):C2}";
                }
                else
                {
                    lblDiscountAmount.Text = "";
                }
            }
            catch
            {
                lblFinalPrice.Text = "Итоговая цена: -";
                lblDiscountAmount.Text = "";
            }
        }

        private void AddEditProductForm_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text))
                txtName.Focus();

            UpdateFinalPrice();
        }
    }
}