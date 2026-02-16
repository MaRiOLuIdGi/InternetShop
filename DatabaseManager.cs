using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace InternetShop
{
    public class DatabaseManager
    {
        private static DatabaseManager instance;
        private string connectionString;
        public bool IsConnected { get; private set; }

        private DatabaseManager()
        {
            // ЗАМЕНИТЕ НА СВОЙ ПАРОЛЬ!
            connectionString = "Server=localhost;Database=InternetShopDB;Uid=root;Pwd=12345dasha;Port=3306;Charset=utf8;";
            IsConnected = false;
        }

        public static DatabaseManager Instance => instance ?? (instance = new DatabaseManager());

        public bool Connect()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    IsConnected = true;
                    CheckAndCreateTable();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось подключиться к MySQL: {ex.Message}\nРаботаем в локальном режиме.",
                               "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                IsConnected = false;
                return false;
            }
        }

        private void CheckAndCreateTable()
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string checkTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Products (
                            id INT AUTO_INCREMENT PRIMARY KEY,
                            name VARCHAR(100) NOT NULL,
                            base_price DECIMAL(10, 2) NOT NULL,
                            has_discount BOOLEAN DEFAULT FALSE,
                            discount_percent DECIMAL(5, 2) DEFAULT 0.00,
                            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                        )";

                    using (var cmd = new MySqlCommand(checkTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public DataTable LoadProducts()
        {
            var dt = new DataTable();

            if (!IsConnected)
            {
                // Создаем структуру таблицы для локального режима
                dt.Columns.Add("id", typeof(int));
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("base_price", typeof(decimal));
                dt.Columns.Add("has_discount", typeof(bool));
                dt.Columns.Add("discount_percent", typeof(decimal));
                dt.Columns.Add("created_at", typeof(DateTime));
                return dt;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            id,
                            name,
                            base_price,
                            has_discount,
                            discount_percent,
                            created_at
                        FROM Products 
                        ORDER BY id";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки товаров: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        public bool AddProduct(string name, double basePrice, bool hasDiscount, double discountPercent)
        {
            if (!IsConnected) return false;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        INSERT INTO Products (name, base_price, has_discount, discount_percent) 
                        VALUES (@name, @price, @hasDiscount, @discount)";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", basePrice);
                        cmd.Parameters.AddWithValue("@hasDiscount", hasDiscount ? 1 : 0);
                        cmd.Parameters.AddWithValue("@discount", discountPercent);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка добавления товара: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool UpdateProduct(int id, string name, double basePrice, bool hasDiscount, double discountPercent)
        {
            if (!IsConnected) return false;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        UPDATE Products 
                        SET name = @name, 
                            base_price = @price, 
                            has_discount = @hasDiscount, 
                            discount_percent = @discount
                        WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", basePrice);
                        cmd.Parameters.AddWithValue("@hasDiscount", hasDiscount ? 1 : 0);
                        cmd.Parameters.AddWithValue("@discount", discountPercent);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обновления товара: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool DeleteProduct(int id)
        {
            if (!IsConnected) return false;

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "DELETE FROM Products WHERE id = @id";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        return cmd.ExecuteNonQuery() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления товара: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public DataTable FindCheapestProduct()
        {
            var dt = new DataTable();

            if (!IsConnected)
            {
                // Для локального режима ищем в текущей таблице
                MessageBox.Show("В локальном режиме используйте поиск через интерфейс",
                               "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return dt;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT * FROM Products 
                        WHERE base_price * (1 - IF(has_discount, discount_percent/100, 0)) = 
                            (SELECT MIN(base_price * (1 - IF(has_discount, discount_percent/100, 0))) 
                             FROM Products)";

                    using (var cmd = new MySqlCommand(query, conn))
                    using (var adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска товара: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        public DataTable SearchProducts(string searchTerm)
        {
            var dt = new DataTable();

            if (!IsConnected)
            {
                MessageBox.Show("В локальном режиме используйте поиск через интерфейс",
                               "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return dt;
            }

            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            id,
                            name,
                            base_price,
                            has_discount,
                            discount_percent,
                            created_at
                        FROM Products 
                        WHERE name LIKE @searchTerm
                        ORDER BY name";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@searchTerm", $"%{searchTerm}%");

                        using (var adapter = new MySqlDataAdapter(cmd))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }

        public bool SaveToCsv(string filePath, DataTable productsData)
        {
            try
            {
                if (productsData == null || productsData.Rows.Count == 0)
                {
                    MessageBox.Show("Нет данных для сохранения",
                                   "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                using (var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    // Записываем заголовки с разделителем точка с запятой
                    writer.WriteLine("ID;Название;Базовая цена;Есть скидка;Скидка %;Итоговая цена;Дата добавления");

                    foreach (DataRow row in productsData.Rows)
                    {
                        // Получаем значения из строки
                        string id = row["id"].ToString();
                        string name = row["name"]?.ToString() ?? "";

                        // Базовую цену форматируем без пробелов и символа валюты
                        double basePrice = Convert.ToDouble(row["base_price"]);
                        string basePriceStr = basePrice.ToString("F2").Replace(",", ".");

                        bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                        string hasDiscountStr = hasDiscount ? "Да" : "Нет";

                        double discountPercent = Convert.ToDouble(row["discount_percent"]);
                        string discountStr = discountPercent.ToString("F1").Replace(",", ".");

                        // Вычисляем итоговую цену
                        double finalPrice = hasDiscount ?
                            basePrice * (1 - discountPercent / 100.0) :
                            basePrice;
                        string finalPriceStr = finalPrice.ToString("F2").Replace(",", ".");

                        // Форматируем дату
                        string dateStr = row["created_at"] != DBNull.Value ?
                            Convert.ToDateTime(row["created_at"]).ToString("yyyy-MM-dd HH:mm:ss") :
                            DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        // Экранируем специальные символы
                        string escapedName = EscapeCsvField(name);
                        string escapedId = EscapeCsvField(id);
                        string escapedDiscountStr = EscapeCsvField(hasDiscountStr);
                        string escapedDateStr = EscapeCsvField(dateStr);

                        // Формируем строку CSV
                        string line = $"{escapedId};" +
                                     $"{escapedName};" +
                                     $"{basePriceStr};" +
                                     $"{escapedDiscountStr};" +
                                     $"{discountStr};" +
                                     $"{finalPriceStr};" +
                                     $"{escapedDateStr}";

                        writer.WriteLine(line);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            // Если поле содержит разделитель, кавычки или перевод строки, заключаем в кавычки
            if (field.Contains(';') || field.Contains('"') || field.Contains('\n') || field.Contains('\r'))
            {
                // Заменяем кавычки на двойные кавычки
                field = field.Replace("\"", "\"\"");
                return $"\"{field}\"";
            }

            return field;
        }

        public DataTable GetStatistics(DataTable productsData)
        {
            var dt = new DataTable();

            if (productsData == null || productsData.Rows.Count == 0)
            {
                // Возвращаем пустую таблицу со структурой
                dt.Columns.Add("Всего товаров", typeof(int));
                dt.Columns.Add("Товаров со скидкой", typeof(int));
                dt.Columns.Add("Средняя базовая цена", typeof(decimal));
                dt.Columns.Add("Средняя итоговая цена", typeof(decimal));
                dt.Columns.Add("Минимальная цена", typeof(decimal));
                dt.Columns.Add("Максимальная цена", typeof(decimal));
                dt.Columns.Add("Общая стоимость", typeof(decimal));

                DataRow emptyRow = dt.NewRow();
                emptyRow["Всего товаров"] = 0;
                emptyRow["Товаров со скидкой"] = 0;
                emptyRow["Средняя базовая цена"] = 0;
                emptyRow["Средняя итоговая цена"] = 0;
                emptyRow["Минимальная цена"] = 0;
                emptyRow["Максимальная цена"] = 0;
                emptyRow["Общая стоимость"] = 0;
                dt.Rows.Add(emptyRow);

                return dt;
            }

            try
            {
                dt.Columns.Add("Всего товаров", typeof(int));
                dt.Columns.Add("Товаров со скидкой", typeof(int));
                dt.Columns.Add("Средняя базовая цена", typeof(decimal));
                dt.Columns.Add("Средняя итоговая цена", typeof(decimal));
                dt.Columns.Add("Минимальная цена", typeof(decimal));
                dt.Columns.Add("Максимальная цена", typeof(decimal));
                dt.Columns.Add("Общая стоимость", typeof(decimal));

                int totalCount = productsData.Rows.Count;
                int discountedCount = 0;
                double totalBasePrice = 0;
                double totalFinalPrice = 0;
                double minFinalPrice = double.MaxValue;
                double maxFinalPrice = double.MinValue;
                double totalSum = 0;

                foreach (DataRow row in productsData.Rows)
                {
                    double basePrice = Convert.ToDouble(row["base_price"]);
                    bool hasDiscount = Convert.ToBoolean(row["has_discount"]);
                    double discountPercent = Convert.ToDouble(row["discount_percent"]);

                    if (hasDiscount) discountedCount++;

                    double finalPrice = hasDiscount ?
                        basePrice * (1 - discountPercent / 100.0) :
                        basePrice;

                    totalBasePrice += basePrice;
                    totalFinalPrice += finalPrice;
                    totalSum += finalPrice;

                    if (finalPrice < minFinalPrice) minFinalPrice = finalPrice;
                    if (finalPrice > maxFinalPrice) maxFinalPrice = finalPrice;
                }

                DataRow statRow = dt.NewRow();
                statRow["Всего товаров"] = totalCount;
                statRow["Товаров со скидкой"] = discountedCount;
                statRow["Средняя базовая цена"] = totalCount > 0 ? (decimal)(totalBasePrice / totalCount) : 0;
                statRow["Средняя итоговая цена"] = totalCount > 0 ? (decimal)(totalFinalPrice / totalCount) : 0;
                statRow["Минимальная цена"] = totalCount > 0 ? (decimal)minFinalPrice : 0;
                statRow["Максимальная цена"] = totalCount > 0 ? (decimal)maxFinalPrice : 0;
                statRow["Общая стоимость"] = (decimal)totalSum;

                dt.Rows.Add(statRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка получения статистики: {ex.Message}",
                               "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }
    }
}