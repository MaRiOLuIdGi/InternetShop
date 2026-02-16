using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace InternetShop
{
    public partial class MainForm : Form
    {
        private DatabaseManager dbManager;
        private DataTable productsTable;
        private Shop shop;
        private string lastSavedFilePath = "";

        public MainForm()
        {
            InitializeComponent();
            InitializeShop();
            SetupDataGridView();

            dbManager = DatabaseManager.Instance;

            if (!dbManager.Connect())
            {
                MessageBox.Show("Работаем в локальном режиме без БД",
                               "Информация",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }

            LoadProducts();
        }

        private void InitializeShop()
        {
            shop = new Shop("Мой Интернет-магазин");
        }

        private void SetupDataGridView()
        {
            dataGridViewProducts.AutoGenerateColumns = false;
            dataGridViewProducts.Columns.Clear();

            // Настраиваем колонки
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "id",
                HeaderText = "ID",
                Width = 50,
                Name = "id",
                ReadOnly = true
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "name",
                HeaderText = "Название товара",
                Width = 200,
                Name = "name",
                ReadOnly = true
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "base_price",
                HeaderText = "Базовая цена",
                Width = 100,
                Name = "base_price",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridViewProducts.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "has_discount",
                HeaderText = "Скидка",
                Width = 70,
                Name = "has_discount",
                ReadOnly = true
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "discount_percent",
                HeaderText = "Скидка %",
                Width = 80,
                Name = "discount_percent",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "F1" }
            });

            // Колонка для итоговой цены (вычисляемое поле)
            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "final_price",
                HeaderText = "Итоговая цена",
                Width = 120,
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "C2" }
            });

            dataGridViewProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "created_at",
                HeaderText = "Дата добавления",
                Width = 150,
                Name = "created_at",
                ReadOnly = true,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd.MM.yyyy HH:mm" }
            });

            // Настройка стилей
            dataGridViewProducts.RowHeadersVisible = false;
            dataGridViewProducts.AllowUserToAddRows = false;
            dataGridViewProducts.AllowUserToDeleteRows = false;
            dataGridViewProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewProducts.MultiSelect = false;
            dataGridViewProducts.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);

            // Добавляем обработчик двойного клика
            dataGridViewProducts.CellDoubleClick += dataGridViewProducts_CellDoubleClick;
        }

        private void LoadProducts()
        {
            try
            {
                productsTable = dbManager.LoadProducts();

                if (!dbManager.IsConnected)
                {
                    // Если нет подключения к БД, показываем демо-данные
                    if (productsTable.Rows.Count == 0)
                    {
                        AddSampleProducts();
                    }
                }

                dataGridViewProducts.DataSource = null;
                dataGridViewProducts.DataSource = productsTable;

                // Добавляем вычисляемую колонку с итоговой ценой
                AddFinalPriceColumn();
                UpdateStatusLabel();
                UpdateShopInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                               "Ошибка",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void AddSampleProducts()
        {
            // Очищаем таблицу
            productsTable.Clear();

            // Добавляем демо-данные
            AddSampleRow(1, "Смартфон Xiaomi Redmi Note 12", 25000, true, 15.5);
            AddSampleRow(2, "Ноутбук Asus Vivobook 15", 55000, false, 0);
            AddSampleRow(3, "Наушники JBL Tune 510BT", 5000, true, 10);
            AddSampleRow(4, "Монитор Samsung 24\"", 30000, false, 0);
            AddSampleRow(5, "Клавиатура Redragon K552", 4000, true, 20);
            AddSampleRow(6, "Мышь Logitech G102", 2500, true, 5);
            AddSampleRow(7, "Принтер HP LaserJet", 12000, false, 0);
            AddSampleRow(8, "Флешка Kingston 64GB", 800, false, 0);
            AddSampleRow(9, "Внешний жесткий диск 1TB", 6000, true, 12.5);
            AddSampleRow(10, "Роутер TP-Link Archer", 3500, false, 0);

            // Обновляем магазин
            shop.ClearProducts();
            foreach (DataRow row in productsTable.Rows)
            {
                shop.AddProduct(new Product(
                    row["name"].ToString(),
                    Convert.ToDouble(row["base_price"]),
                    Convert.ToBoolean(row["has_discount"]),
                    Convert.ToDouble(row["discount_percent"])
                ));
            }
        }

        private void AddSampleRow(int id, string name, double price, bool hasDiscount, double discount)
        {
            DataRow row = productsTable.NewRow();
            row["id"] = id;
            row["name"] = name;
            row["base_price"] = price;
            row["has_discount"] = hasDiscount;
            row["discount_percent"] = discount;
            row["created_at"] = DateTime.Now;
            productsTable.Rows.Add(row);
        }

        private void AddFinalPriceColumn()
        {
            foreach (DataGridViewRow row in dataGridViewProducts.Rows)
            {
                if (row.IsNewRow) continue;

                try
                {
                    // Получаем данные из ячеек
                    double basePrice = 0;
                    bool hasDiscount = false;
                    double discountPercent = 0;

                    var basePriceCell = row.Cells["base_price"]?.Value;
                    var hasDiscountCell = row.Cells["has_discount"]?.Value;
                    var discountCell = row.Cells["discount_percent"]?.Value;

                    if (basePriceCell != null && basePriceCell != DBNull.Value)
                        double.TryParse(basePriceCell.ToString(), out basePrice);

                    if (hasDiscountCell != null && hasDiscountCell != DBNull.Value)
                        hasDiscount = Convert.ToBoolean(hasDiscountCell);

                    if (discountCell != null && discountCell != DBNull.Value)
                        double.TryParse(discountCell.ToString(), out discountPercent);

                    // Вычисляем итоговую цену
                    double finalPrice = hasDiscount ?
                        basePrice * (1 - discountPercent / 100.0) :
                        basePrice;

                    // Устанавливаем значение в ячейку
                    row.Cells["final_price"].Value = finalPrice;
                }
                catch
                {
                    row.Cells["final_price"].Value = 0;
                }
            }
        }

        private void UpdateStatusLabel()
        {
            int productCount = productsTable?.Rows.Count ?? 0;
            string status = dbManager.IsConnected ?
                $"✅ Подключено к БД. Товаров: {productCount}" :
                $"⚠️ Локальный режим. Демо-данные: {productCount}";

            lblStatus.Text = status;
        }

        private void UpdateShopInfo()
        {
            lblShopName.Text = shop.ShopName;

            if (productsTable != null)
            {
                int productCount = productsTable.Rows.Count;
                int discountedCount = 0;
                double totalValue = 0;

                foreach (DataRow row in productsTable.Rows)
                {
                    double basePrice = Convert.ToDouble(row["base_price"]);
                    bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                    double discountPercent = Convert.ToDouble(row["discount_percent"]);

                    if (hasDiscount) discountedCount++;

                    double finalPrice = hasDiscount ?
                        basePrice * (1 - discountPercent / 100.0) :
                        basePrice;

                    totalValue += finalPrice;
                }

                lblProductCount.Text = $"📦 Товаров: {productCount}";
                lblDiscountedCount.Text = $"🏷️ Со скидкой: {discountedCount}";
                lblTotalValue.Text = $"💰 Общая стоимость: {totalValue:C2}";

                // Обновляем объект магазина
                shop.ClearProducts();
                foreach (DataRow row in productsTable.Rows)
                {
                    try
                    {
                        shop.AddProduct(new Product(
                            row["name"].ToString(),
                            Convert.ToDouble(row["base_price"]),
                            Convert.ToBoolean(row["has_discount"]),
                            Convert.ToDouble(row["discount_percent"])
                        ));
                    }
                    catch { }
                }
            }
            else
            {
                lblProductCount.Text = "📦 Товаров: 0";
                lblDiscountedCount.Text = "🏷️ Со скидкой: 0";
                lblTotalValue.Text = "💰 Общая стоимость: 0.00 ₽";
            }
        }

        // МЕТОД ДЛЯ КНОПКИ "ДОБАВИТЬ ТОВАР"
        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditProductForm();
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string name = addForm.ProductName;
                    double price = addForm.Price;
                    bool hasDiscount = addForm.HasDiscount;
                    double discount = addForm.DiscountPercent;

                    // Добавляем в магазин
                    Product product = new Product(name, price, hasDiscount, discount);
                    shop.AddProduct(product);

                    // Если подключены к БД, добавляем туда
                    if (dbManager.IsConnected)
                    {
                        bool success = dbManager.AddProduct(name, price, hasDiscount, discount);
                        if (!success)
                        {
                            MessageBox.Show("Не удалось добавить товар в базу данных, но он сохранен локально",
                                           "Предупреждение",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Warning);
                        }
                    }

                    // Добавляем в таблицу
                    DataRow newRow = productsTable.NewRow();
                    newRow["id"] = dbManager.IsConnected ? GetNextId() : productsTable.Rows.Count + 1;
                    newRow["name"] = name;
                    newRow["base_price"] = price;
                    newRow["has_discount"] = hasDiscount;
                    newRow["discount_percent"] = discount;
                    newRow["created_at"] = DateTime.Now;
                    productsTable.Rows.Add(newRow);

                    // Обновляем отображение
                    dataGridViewProducts.Refresh();
                    AddFinalPriceColumn();
                    UpdateStatusLabel();
                    UpdateShopInfo();

                    MessageBox.Show("✅ Товар успешно добавлен!",
                                   "Успех",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Ошибка при добавлении товара: {ex.Message}",
                                   "Ошибка",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }
            }
        }

        private int GetNextId()
        {
            int maxId = 0;
            foreach (DataRow row in productsTable.Rows)
            {
                int id = Convert.ToInt32(row["id"]);
                if (id > maxId) maxId = id;
            }
            return maxId + 1;
        }

        private void btnEditProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для редактирования",
                               "Внимание",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridViewProducts.SelectedRows[0];
            int rowIndex = selectedRow.Index;

            if (rowIndex < 0 || rowIndex >= productsTable.Rows.Count)
                return;

            try
            {
                var editForm = new AddEditProductForm(true);

                DataRow row = productsTable.Rows[rowIndex];
                string name = row["name"].ToString();
                double price = Convert.ToDouble(row["base_price"]);
                bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                double discount = Convert.ToDouble(row["discount_percent"]);
                int id = Convert.ToInt32(row["id"]);

                editForm.SetData(name, price, hasDiscount, discount);

                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    if (dbManager.IsConnected)
                    {
                        bool success = dbManager.UpdateProduct(id,
                            editForm.ProductName,
                            editForm.Price,
                            editForm.HasDiscount,
                            editForm.DiscountPercent);

                        if (!success)
                        {
                            MessageBox.Show("Не удалось обновить товар в базе данных, но изменения сохранены локально",
                                           "Предупреждение",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Warning);
                        }
                    }

                    // Обновляем строку в таблице
                    row["name"] = editForm.ProductName;
                    row["base_price"] = editForm.Price;
                    row["has_discount"] = editForm.HasDiscount;
                    row["discount_percent"] = editForm.DiscountPercent;

                    // Обновляем отображение
                    dataGridViewProducts.Refresh();
                    AddFinalPriceColumn();
                    UpdateShopInfo();

                    MessageBox.Show("✅ Товар успешно обновлен!",
                                   "Успех",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка при редактировании товара: {ex.Message}",
                               "Ошибка",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            if (dataGridViewProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите товар для удаления",
                               "Внимание",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            DataGridViewRow selectedRow = dataGridViewProducts.SelectedRows[0];
            string productName = selectedRow.Cells["name"].Value?.ToString() ?? "товар";

            if (MessageBox.Show($"Удалить товар \"{productName}\"?",
                               "Подтверждение удаления",
                               MessageBoxButtons.YesNo,
                               MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                try
                {
                    int rowIndex = selectedRow.Index;

                    if (rowIndex < 0 || rowIndex >= productsTable.Rows.Count)
                        return;

                    DataRow row = productsTable.Rows[rowIndex];
                    int id = Convert.ToInt32(row["id"]);

                    // Удаляем из БД если подключены
                    if (dbManager.IsConnected)
                    {
                        bool success = dbManager.DeleteProduct(id);
                        if (!success)
                        {
                            MessageBox.Show("Не удалось удалить товар из базы данных, но он удален локально",
                                           "Предупреждение",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Warning);
                        }
                    }

                    // Удаляем из таблицы
                    productsTable.Rows.RemoveAt(rowIndex);

                    // Обновляем отображение
                    dataGridViewProducts.Refresh();
                    AddFinalPriceColumn();
                    UpdateStatusLabel();
                    UpdateShopInfo();

                    MessageBox.Show("✅ Товар успешно удален!",
                                   "Успех",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"❌ Ошибка при удалении товара: {ex.Message}",
                                   "Ошибка",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                }
            }
        }

        private void btnFindCheapest_Click(object sender, EventArgs e)
        {
            try
            {
                if (productsTable == null || productsTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет товаров для поиска", "Информация",
                                   MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (dbManager.IsConnected)
                {
                    DataTable cheapestProducts = dbManager.FindCheapestProduct();

                    if (cheapestProducts.Rows.Count > 0)
                    {
                        ShowCheapestProductDialog(cheapestProducts);
                    }
                    else
                    {
                        MessageBox.Show("Товары не найдены", "Информация",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Локальный поиск
                    var cheapest = shop.FindCheapestProduct();
                    if (cheapest != null)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("name");
                        dt.Columns.Add("base_price");
                        dt.Columns.Add("has_discount");
                        dt.Columns.Add("discount_percent");

                        DataRow row = dt.NewRow();
                        row["name"] = cheapest.Name;
                        row["base_price"] = cheapest.BasePrice;
                        row["has_discount"] = cheapest.HasDiscount;
                        row["discount_percent"] = cheapest.DiscountPercent;
                        dt.Rows.Add(row);

                        ShowCheapestProductDialog(dt);
                    }
                    else
                    {
                        MessageBox.Show("Товары не найдены", "Информация",
                                       MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске товаров: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowCheapestProductDialog(DataTable cheapestProducts)
        {
            string message = "🏆 САМЫЙ ДЕШЕВЫЙ ТОВАР\n";
            message += "══════════════════════\n\n";

            foreach (DataRow row in cheapestProducts.Rows)
            {
                string name = row["name"].ToString();
                double basePrice = Convert.ToDouble(row["base_price"]);
                bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                double discountPercent = Convert.ToDouble(row["discount_percent"]);

                double finalPrice = hasDiscount ?
                    basePrice * (1 - discountPercent / 100.0) :
                    basePrice;

                message += $"📦 {name}\n";
                message += $"   Базовая цена: {basePrice:C2}\n";
                if (hasDiscount)
                {
                    message += $"   Скидка: {discountPercent:F1}%\n";
                    message += $"   Экономия: {(basePrice - finalPrice):C2}\n";
                }
                message += $"   💰 Итоговая цена: {finalPrice:C2}\n\n";
            }

            MessageBox.Show(message, "Результат поиска",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchTerm))
            {
                LoadProducts();
                return;
            }

            try
            {
                if (dbManager.IsConnected)
                {
                    // Поиск в БД
                    DataTable searchResults = dbManager.SearchProducts(searchTerm);

                    if (searchResults.Rows.Count > 0)
                    {
                        productsTable = searchResults;
                        dataGridViewProducts.DataSource = productsTable;
                        AddFinalPriceColumn();
                        lblStatus.Text = $"🔍 Результаты поиска: {searchResults.Rows.Count} товаров";
                        UpdateShopInfo();

                        MessageBox.Show($"Найдено товаров: {searchResults.Rows.Count}",
                                       "Результат поиска",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"Товары, содержащие '{searchTerm}', не найдены",
                                       "Результат поиска",
                                       MessageBoxButtons.OK,
                                       MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Локальный поиск по DataTable
                    try
                    {
                        DataView dv = productsTable.DefaultView;
                        dv.RowFilter = $"name LIKE '%{searchTerm.Replace("'", "''")}%'";

                        if (dv.Count > 0)
                        {
                            DataTable searchResults = dv.ToTable();
                            dataGridViewProducts.DataSource = searchResults;
                            AddFinalPriceColumn();
                            lblStatus.Text = $"🔍 Найдено товаров: {searchResults.Rows.Count}";
                            UpdateShopInfo();

                            MessageBox.Show($"Найдено товаров: {searchResults.Rows.Count}",
                                           "Результат поиска",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Товары, содержащие '{searchTerm}', не найдены",
                                           "Результат поиска",
                                           MessageBoxButtons.OK,
                                           MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при фильтрации: {ex.Message}",
                                       "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при поиске: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // НОВЫЙ МЕТОД: Загрузка данных из файла в приложение (УЛУЧШЕННАЯ ВЕРСИЯ)
        private void btnLoadFromFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Выберите файл для загрузки данных";
                openDialog.Filter = "Текстовые файлы (*.txt)|*.txt|CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                openDialog.FilterIndex = 1;
                openDialog.RestoreDirectory = true;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Проверяем существование файла
                        if (!File.Exists(openDialog.FileName))
                        {
                            MessageBox.Show("Файл не существует!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Показываем содержимое файла для отладки
                        string fileContent = File.ReadAllText(openDialog.FileName, Encoding.UTF8);
                        MessageBox.Show($"Содержимое файла (первые 500 символов):\n\n{fileContent.Substring(0, Math.Min(500, fileContent.Length))}",
                            "Содержимое файла", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Загружаем данные из файла
                        DataTable loadedData = LoadDataFromFile(openDialog.FileName);

                        if (loadedData != null && loadedData.Rows.Count > 0)
                        {
                            // Спрашиваем, хотим ли заменить данные или добавить
                            DialogResult result = MessageBox.Show(
                                $"Найдено {loadedData.Rows.Count} записей в файле.\n\n" +
                                "Хотите ЗАМЕНИТЬ текущие данные? (Нажмите НЕТ, чтобы ДОБАВИТЬ к существующим)",
                                "Загрузка данных",
                                MessageBoxButtons.YesNoCancel,
                                MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                // Заменяем данные
                                productsTable = loadedData;
                                shop.ClearProducts();
                                foreach (DataRow row in productsTable.Rows)
                                {
                                    try
                                    {
                                        shop.AddProduct(new Product(
                                            row["name"].ToString(),
                                            Convert.ToDouble(row["base_price"]),
                                            Convert.ToBoolean(row["has_discount"]),
                                            Convert.ToDouble(row["discount_percent"])
                                        ));
                                    }
                                    catch { }
                                }

                                MessageBox.Show($"Данные заменены. Загружено {loadedData.Rows.Count} записей.",
                                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (result == DialogResult.No)
                            {
                                // Добавляем данные
                                int addedCount = 0;
                                int maxId = GetNextId();

                                foreach (DataRow row in loadedData.Rows)
                                {
                                    try
                                    {
                                        string name = row["name"].ToString();
                                        double price = Convert.ToDouble(row["base_price"]);
                                        bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                                        double discount = Convert.ToDouble(row["discount_percent"]);

                                        // Проверяем, нет ли уже такого товара (по имени)
                                        bool exists = false;
                                        foreach (DataRow existingRow in productsTable.Rows)
                                        {
                                            if (existingRow["name"].ToString().Equals(name, StringComparison.OrdinalIgnoreCase))
                                            {
                                                exists = true;
                                                break;
                                            }
                                        }

                                        if (!exists)
                                        {
                                            DataRow newRow = productsTable.NewRow();
                                            newRow["id"] = maxId++;
                                            newRow["name"] = name;
                                            newRow["base_price"] = price;
                                            newRow["has_discount"] = hasDiscount;
                                            newRow["discount_percent"] = discount;
                                            newRow["created_at"] = DateTime.Now;
                                            productsTable.Rows.Add(newRow);

                                            shop.AddProduct(new Product(name, price, hasDiscount, discount));
                                            addedCount++;
                                        }
                                    }
                                    catch { }
                                }

                                MessageBox.Show($"Добавлено {addedCount} новых записей.",
                                    "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                return; // Отмена
                            }

                            // Обновляем отображение
                            dataGridViewProducts.DataSource = null;
                            dataGridViewProducts.DataSource = productsTable;
                            AddFinalPriceColumn();
                            UpdateStatusLabel();
                            UpdateShopInfo();

                            lastSavedFilePath = openDialog.FileName;
                        }
                        else
                        {
                            MessageBox.Show("Не удалось загрузить данные из файла или файл пуст.",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}\n\n{ex.StackTrace}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // НОВЫЙ МЕТОД: Загрузка данных из текстового файла (УЛУЧШЕННАЯ ВЕРСИЯ)
        private DataTable LoadDataFromFile(string filePath)
        {
            DataTable dt = new DataTable();

            try
            {
                // Создаем структуру таблицы
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("base_price", typeof(double));
                dt.Columns.Add("has_discount", typeof(bool));
                dt.Columns.Add("discount_percent", typeof(double));
                dt.Columns.Add("created_at", typeof(DateTime));

                string[] lines = File.ReadAllLines(filePath, Encoding.UTF8);

                int parsedRows = 0;

                foreach (string line in lines)
                {
                    // Пропускаем пустые строки
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    // Пропускаем строки с разделителями и заголовками
                    if (line.StartsWith("=") || line.StartsWith("-") ||
                        line.Contains("ОТЧЕТ") || line.Contains("СТАТИСТИКА") ||
                        line.Contains("Конец файла") || line.Contains("ИТОГОВАЯ") ||
                        line.Contains("Дата создания") || line.Contains("Всего товаров"))
                    {
                        continue;
                    }

                    // Пропускаем заголовки колонок
                    if (line.Contains("ID") && line.Contains("НАЗВАНИЕ") && line.Contains("ЦЕНА"))
                    {
                        continue;
                    }

                    // Пробуем распарсить как CSV (с разделителем ;)
                    if (line.Contains(";"))
                    {
                        string[] parts = line.Split(';');
                        if (parts.Length >= 5)
                        {
                            try
                            {
                                int id = int.Parse(parts[0].Trim());
                                string name = parts[1].Trim().Trim('"');
                                double basePrice = double.Parse(parts[2].Trim().Replace(',', '.'),
                                    System.Globalization.CultureInfo.InvariantCulture);
                                bool hasDiscount = parts[3].Trim() == "Да";
                                double discount = double.Parse(parts[4].Trim().Replace(',', '.'),
                                    System.Globalization.CultureInfo.InvariantCulture);

                                DataRow row = dt.NewRow();
                                row["id"] = id;
                                row["name"] = name;
                                row["base_price"] = basePrice;
                                row["has_discount"] = hasDiscount;
                                row["discount_percent"] = discount;
                                row["created_at"] = DateTime.Now;
                                dt.Rows.Add(row);
                                parsedRows++;
                                continue;
                            }
                            catch { }
                        }
                    }

                    // Парсим форматированный текст
                    try
                    {
                        // Разделяем строку по пробелам
                        string[] parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        if (parts.Length >= 5)
                        {
                            // ID - первый элемент
                            if (int.TryParse(parts[0], out int id))
                            {
                                // Ищем цену и скидку
                                double basePrice = 0;
                                double finalPrice = 0;
                                double discountPercent = 0;
                                bool hasDiscount = false;

                                // Проходим по всем частям строки
                                for (int i = 0; i < parts.Length; i++)
                                {
                                    // Ищем цену (с символом ₽)
                                    if (parts[i].Contains("₽"))
                                    {
                                        string priceStr = parts[i].Replace("₽", "").Replace(",", ".");
                                        if (double.TryParse(priceStr, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out double val))
                                        {
                                            if (basePrice == 0)
                                                basePrice = val;
                                            else
                                                finalPrice = val;
                                        }
                                    }

                                    // Ищем скидку (с символом %)
                                    if (parts[i].Contains("%"))
                                    {
                                        string discountStr = parts[i].Replace("%", "").Replace(",", ".");
                                        if (double.TryParse(discountStr, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out double val))
                                        {
                                            discountPercent = val;
                                            hasDiscount = true;
                                        }
                                    }
                                }

                                // Если не нашли цену через ₽, пробуем найти число в конце строки
                                if (basePrice == 0)
                                {
                                    for (int i = parts.Length - 1; i >= 0; i--)
                                    {
                                        string cleanPart = parts[i].Replace("₽", "").Replace(",", ".");
                                        if (double.TryParse(cleanPart, System.Globalization.NumberStyles.Any,
                                            System.Globalization.CultureInfo.InvariantCulture, out double val))
                                        {
                                            if (val > 100) // Скорее всего это цена
                                            {
                                                basePrice = val;
                                                break;
                                            }
                                        }
                                    }
                                }

                                // Собираем название
                                string name = "";
                                for (int i = 1; i < parts.Length; i++)
                                {
                                    // Пропускаем числа и специальные символы
                                    if (!parts[i].Contains("₽") && !parts[i].Contains("%") &&
                                        !double.TryParse(parts[i].Replace(",", "."), out _))
                                    {
                                        name += parts[i] + " ";
                                    }
                                }
                                name = name.Trim();

                                // Если название пустое, берем все между ID и ценой
                                if (string.IsNullOrEmpty(name) && parts.Length > 3)
                                {
                                    for (int i = 1; i < parts.Length - 2; i++)
                                    {
                                        name += parts[i] + " ";
                                    }
                                    name = name.Trim();
                                }

                                // Если нашли цену и название
                                if (!string.IsNullOrEmpty(name) && basePrice > 0)
                                {
                                    DataRow row = dt.NewRow();
                                    row["id"] = id;
                                    row["name"] = name;
                                    row["base_price"] = basePrice;
                                    row["has_discount"] = hasDiscount;
                                    row["discount_percent"] = discountPercent;
                                    row["created_at"] = DateTime.Now;
                                    dt.Rows.Add(row);
                                    parsedRows++;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Логируем ошибку но продолжаем
                        System.Diagnostics.Debug.WriteLine($"Ошибка парсинга строки: {ex.Message}");
                    }
                }

                MessageBox.Show($"Успешно загружено {parsedRows} записей из файла.",
                    "Результат загрузки", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при парсинге файла: {ex.Message}\n\n{ex.StackTrace}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return dt;
        }

        // МЕТОД: Открытие файла (просто открывает в программе по умолчанию)
        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Title = "Выберите файл для открытия";
                openDialog.Filter = "Текстовые файлы (*.txt)|*.txt|CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                openDialog.FilterIndex = 1;
                openDialog.RestoreDirectory = true;

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        if (!File.Exists(openDialog.FileName))
                        {
                            MessageBox.Show("Файл не существует!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        Process.Start(new ProcessStartInfo
                        {
                            FileName = openDialog.FileName,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при открытии файла: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // МЕТОД: Сортировка по возрастанию цены
        private void btnSortAsc_Click(object sender, EventArgs e)
        {
            try
            {
                if (productsTable == null || productsTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для сортировки", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataView dv = productsTable.DefaultView;
                dv.Sort = "base_price ASC";
                DataTable sortedTable = dv.ToTable();

                dataGridViewProducts.DataSource = sortedTable;
                AddFinalPriceColumn();
                lblStatus.Text = $"📊 Отсортировано по возрастанию цены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // МЕТОД: Сортировка по убыванию цены
        private void btnSortDesc_Click(object sender, EventArgs e)
        {
            try
            {
                if (productsTable == null || productsTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для сортировки", "Информация",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataView dv = productsTable.DefaultView;
                dv.Sort = "base_price DESC";
                DataTable sortedTable = dv.ToTable();

                dataGridViewProducts.DataSource = sortedTable;
                AddFinalPriceColumn();
                lblStatus.Text = $"📊 Отсортировано по убыванию цены";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ОСНОВНОЙ МЕТОД СОХРАНЕНИЯ В ТЕКСТОВЫЙ ФАЙЛ
        private void btnSaveToCsv_Click(object sender, EventArgs e)
        {
            try
            {
                // Проверяем наличие данных
                if (productsTable == null)
                {
                    MessageBox.Show("Ошибка: Таблица с данными не инициализирована",
                        "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (productsTable.Rows.Count == 0)
                {
                    MessageBox.Show("В таблице нет данных для сохранения",
                        "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Настройка диалога сохранения
                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Title = "Сохранить данные как текстовый файл";
                    saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                    saveDialog.FilterIndex = 1;
                    saveDialog.DefaultExt = "txt";
                    saveDialog.AddExtension = true;
                    saveDialog.FileName = $"товары_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Сохраняем файл
                        bool success = SaveToTextFile(saveDialog.FileName, productsTable);

                        if (success)
                        {
                            lastSavedFilePath = saveDialog.FileName;

                            // Проверяем результат
                            FileInfo fileInfo = new FileInfo(saveDialog.FileName);
                            if (fileInfo.Exists)
                            {
                                string content = File.ReadAllText(saveDialog.FileName, Encoding.UTF8);
                                int lineCount = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None).Length;

                                DialogResult result = MessageBox.Show(
                                    $"✅ Файл успешно сохранен!\n\n" +
                                    $"📁 Путь: {saveDialog.FileName}\n" +
                                    $"📊 Размер: {fileInfo.Length} байт\n" +
                                    $"📝 Количество строк: {lineCount}\n" +
                                    $"📦 Записей товаров: {productsTable.Rows.Count}\n\n" +
                                    $"Хотите открыть файл сейчас?",
                                    "Успех",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    Process.Start(new ProcessStartInfo
                                    {
                                        FileName = saveDialog.FileName,
                                        UseShellExecute = true
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка при сохранении: {ex.Message}\n\n{ex.StackTrace}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // МЕТОД СОХРАНЕНИЯ В ТЕКСТОВЫЙ ФАЙЛ
        private bool SaveToTextFile(string filePath, DataTable data)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Заголовок файла
                    writer.WriteLine("=" + new string('=', 80));
                    writer.WriteLine("           ОТЧЕТ О ТОВАРАХ ИНТЕРНЕТ-МАГАЗИНА");
                    writer.WriteLine("=" + new string('=', 80));
                    writer.WriteLine($"Дата создания: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
                    writer.WriteLine($"Всего товаров: {data.Rows.Count}");
                    writer.WriteLine("-" + new string('-', 80));

                    // Заголовки колонок
                    writer.WriteLine($"{"ID",-5} {"НАЗВАНИЕ",-35} {"ЦЕНА",-12} {"СКИДКА",-10} {"ИТОГО",-12}");
                    writer.WriteLine("-" + new string('-', 80));

                    // Данные
                    int rowNumber = 0;
                    foreach (DataRow row in data.Rows)
                    {
                        rowNumber++;

                        string id = row["id"].ToString();
                        string name = row["name"]?.ToString() ?? "";
                        if (name.Length > 33) name = name.Substring(0, 30) + "...";

                        double basePrice = Convert.ToDouble(row["base_price"]);
                        bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                        double discountPercent = 0;

                        if (hasDiscount)
                        {
                            double.TryParse(row["discount_percent"].ToString(), out discountPercent);
                        }

                        double finalPrice = hasDiscount ?
                            basePrice * (1 - discountPercent / 100.0) :
                            basePrice;

                        string discountStr = hasDiscount ? $"{discountPercent:F1}%" : "Нет";

                        writer.WriteLine($"{id,-5} {name,-35} {basePrice,11:F2} ₽ {discountStr,9} {finalPrice,11:F2} ₽");

                        // Каждые 20 строк добавляем разделитель
                        if (rowNumber % 20 == 0 && rowNumber < data.Rows.Count)
                        {
                            writer.WriteLine("-" + new string('-', 80));
                        }
                    }

                    // Итоговая статистика
                    writer.WriteLine("-" + new string('-', 80));

                    // Подсчет статистики
                    int discountedCount = 0;
                    double totalSum = 0;
                    double maxPrice = 0;
                    double minPrice = double.MaxValue;
                    string maxPriceName = "", minPriceName = "";

                    foreach (DataRow row in data.Rows)
                    {
                        double basePrice = Convert.ToDouble(row["base_price"]);
                        bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                        double discountPercent = Convert.ToDouble(row["discount_percent"]);
                        string name = row["name"]?.ToString() ?? "";

                        double finalPrice = hasDiscount ?
                            basePrice * (1 - discountPercent / 100.0) :
                            basePrice;

                        totalSum += finalPrice;

                        if (hasDiscount) discountedCount++;

                        if (finalPrice > maxPrice)
                        {
                            maxPrice = finalPrice;
                            maxPriceName = name;
                        }

                        if (finalPrice < minPrice)
                        {
                            minPrice = finalPrice;
                            minPriceName = name;
                        }
                    }

                    double avgPrice = totalSum / data.Rows.Count;

                    writer.WriteLine($"ИТОГОВАЯ СТАТИСТИКА:");
                    writer.WriteLine($"  • Общая стоимость: {totalSum,14:F2} ₽");
                    writer.WriteLine($"  • Средняя цена: {avgPrice,17:F2} ₽");
                    writer.WriteLine($"  • Товаров со скидкой: {discountedCount}");
                    writer.WriteLine($"  • Самый дорогой: {maxPriceName} - {maxPrice:F2} ₽");
                    writer.WriteLine($"  • Самый дешевый: {minPriceName} - {minPrice:F2} ₽");

                    writer.WriteLine("=" + new string('=', 80));
                    writer.WriteLine($"Конец файла. Всего записей: {data.Rows.Count}");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при записи файла: {ex.Message}",
                    "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            try
            {
                if (productsTable == null || productsTable.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для отображения статистики",
                                   "Информация",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                    return;
                }

                DataTable stats = dbManager.GetStatistics(productsTable);

                if (stats != null && stats.Rows.Count > 0)
                {
                    DataRow stat = stats.Rows[0];

                    decimal avgBasePrice = Convert.ToDecimal(stat["Средняя базовая цена"]);
                    decimal avgFinalPrice = Convert.ToDecimal(stat["Средняя итоговая цена"]);
                    decimal minPrice = Convert.ToDecimal(stat["Минимальная цена"]);
                    decimal maxPrice = Convert.ToDecimal(stat["Максимальная цена"]);
                    decimal totalSum = Convert.ToDecimal(stat["Общая стоимость"]);

                    string message = $"📊 СТАТИСТИКА МАГАЗИНА\n" +
                                   $"═════════════════════════\n\n" +
                                   $"📦 Всего товаров: {stat["Всего товаров"]}\n" +
                                   $"🏷️ Товаров со скидкой: {stat["Товаров со скидкой"]}\n" +
                                   $"💰 Средняя базовая цена: {avgBasePrice:C2}\n" +
                                   $"💵 Средняя итоговая цена: {avgFinalPrice:C2}\n" +
                                   $"⬇️ Минимальная цена: {minPrice:C2}\n" +
                                   $"⬆️ Максимальная цена: {maxPrice:C2}\n" +
                                   $"💳 Общая стоимость: {totalSum:C2}";

                    MessageBox.Show(message, "Статистика",
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Ошибка при получении статистики: {ex.Message}",
                               "Ошибка",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
            txtSearch.Clear();
            MessageBox.Show("Список товаров обновлен", "Обновление",
                           MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAddWithDiscount_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditProductForm();
            addForm.SetDiscountMode(true);
            addForm.Text = "➕ Добавить товар со скидкой";

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                btnAddProduct_Click(sender, e);
            }
        }

        private void btnAddWithoutDiscount_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditProductForm();
            addForm.SetDiscountMode(false);
            addForm.Text = "➕ Добавить товар без скидки";

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                btnAddProduct_Click(sender, e);
            }
        }

        private void dataGridViewProducts_SelectionChanged(object sender, EventArgs e)
        {
            // Отображаем информацию о выбранном товаре в статусной строке
            if (dataGridViewProducts.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridViewProducts.SelectedRows[0];
                string name = row.Cells["name"]?.Value?.ToString() ?? "";
                var price = row.Cells["final_price"]?.Value;

                if (price != null)
                {
                    lblStatus.Text = $"✅ Выбран: {name} - Цена: {Convert.ToDouble(price):C2}";
                }
            }
        }

        private void dataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditProduct_Click(sender, EventArgs.Empty);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Delete && dataGridViewProducts.SelectedRows.Count > 0)
            {
                btnDeleteProduct_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == Keys.F5)
            {
                btnRefresh_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == (Keys.Control | Keys.F))
            {
                txtSearch.Focus();
                txtSearch.SelectAll();
                return true;
            }
            if (keyData == (Keys.Control | Keys.S))
            {
                btnSaveToCsv_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == (Keys.Control | Keys.O))
            {
                btnOpenFile_Click(this, EventArgs.Empty);
                return true;
            }
            if (keyData == (Keys.Control | Keys.L))
            {
                btnLoadFromFile_Click(this, EventArgs.Empty);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}