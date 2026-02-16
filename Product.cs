using System;

namespace InternetShop
{
    public class Product
    {
        private string name;
        private double basePrice;
        private bool hasDiscount;
        private double discountPercent;

        // Конструктор по умолчанию
        public Product() : this("Новый товар", 0.0, false, 0.0) { }

        // Основной конструктор
        public Product(string productName, double price, bool discount = false, double discountValue = 0.0)
        {
            ValidateData(productName, price, discountValue);

            name = productName;
            basePrice = price;
            hasDiscount = discount;
            discountPercent = discountValue;
        }

        // Валидация данных
        private void ValidateData(string productName, double price, double discountValue)
        {
            if (string.IsNullOrWhiteSpace(productName))
                throw new ArgumentException("Название товара не может быть пустым");

            if (productName.Length > 100)
                throw new ArgumentException("Название товара не может превышать 100 символов");

            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной");

            if (price > 1000000)
                throw new ArgumentException("Цена не может превышать 1 000 000");

            if (discountValue < 0 || discountValue > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%");
        }

        // Свойства
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Название товара не может быть пустым");
                if (value.Length > 100)
                    throw new ArgumentException("Название товара не может превышать 100 символов");
                name = value;
            }
        }

        public double BasePrice
        {
            get => basePrice;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Цена не может быть отрицательной");
                if (value > 1000000)
                    throw new ArgumentException("Цена не может превышать 1 000 000");
                basePrice = value;
            }
        }

        public bool HasDiscount
        {
            get => hasDiscount;
            set => hasDiscount = value;
        }

        public double DiscountPercent
        {
            get => discountPercent;
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Скидка должна быть от 0 до 100%");
                discountPercent = value;
            }
        }

        // Метод для расчета итоговой цены
        public double CalculateFinalPrice()
        {
            if (hasDiscount)
                return basePrice * (1 - discountPercent / 100.0);
            return basePrice;
        }

        // Переопределение ToString
        public override string ToString()
        {
            return $"{name} - {CalculateFinalPrice():C2}" +
                   (hasDiscount ? $" (скидка {discountPercent}%)" : "");
        }

        // Метод для получения информации о товаре
        public string GetProductInfo()
        {
            return $"Название: {name}\n" +
                   $"Базовая цена: {basePrice:C2}\n" +
                   $"Скидка: {(hasDiscount ? $"Да ({discountPercent}%)" : "Нет")}\n" +
                   $"Итоговая цена: {CalculateFinalPrice():C2}";
        }
    }
}