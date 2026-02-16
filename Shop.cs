using System;
using System.Collections.Generic;
using System.Linq;

namespace InternetShop
{
    public class Shop
    {
        private List<Product> products;
        private string shopName;

        public Shop(string name = "Интернет-магазин")
        {
            shopName = name;
            products = new List<Product>();
        }

        // Свойство для названия магазина
        public string ShopName
        {
            get => shopName;
            set => shopName = value;
        }

        // Свойство для списка товаров
        public List<Product> Products => products;

        // Метод добавления товара со скидкой
        public void AddProductWithDiscount(string name, double price, double discountPercent)
        {
            if (discountPercent < 0 || discountPercent > 100)
                throw new ArgumentException("Скидка должна быть от 0 до 100%");

            Product product = new Product(name, price, true, discountPercent);
            products.Add(product);
        }

        // Метод добавления товара без скидки
        public void AddProductWithoutDiscount(string name, double price)
        {
            Product product = new Product(name, price, false, 0);
            products.Add(product);
        }

        // Метод добавления существующего объекта Product
        public void AddProduct(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Товар не может быть null");

            products.Add(product);
        }

        // Метод поиска товара с минимальной стоимостью
        public Product FindCheapestProduct()
        {
            if (products.Count == 0)
                return null;

            return products.OrderBy(p => p.CalculateFinalPrice()).First();
        }

        // Метод поиска товара с максимальной стоимостью
        public Product FindMostExpensiveProduct()
        {
            if (products.Count == 0)
                return null;

            return products.OrderByDescending(p => p.CalculateFinalPrice()).First();
        }

        // Метод поиска товаров со скидкой
        public List<Product> GetDiscountedProducts()
        {
            return products.Where(p => p.HasDiscount).ToList();
        }

        // Метод поиска товаров по названию
        public List<Product> SearchProductsByName(string searchTerm)
        {
            return products.Where(p => p.Name.ToLower().Contains(searchTerm.ToLower())).ToList();
        }

        // Метод получения общей стоимости всех товаров
        public double GetTotalInventoryValue()
        {
            return products.Sum(p => p.CalculateFinalPrice());
        }

        // Метод получения количества товаров
        public int GetProductCount()
        {
            return products.Count;
        }

        // Метод получения количества товаров со скидкой
        public int GetDiscountedProductCount()
        {
            return products.Count(p => p.HasDiscount);
        }

        // Метод удаления товара по индексу
        public bool RemoveProduct(int index)
        {
            if (index >= 0 && index < products.Count)
            {
                products.RemoveAt(index);
                return true;
            }
            return false;
        }

        // Метод очистки списка товаров
        public void ClearProducts()
        {
            products.Clear();
        }

        // Метод получения средней цены товаров
        public double GetAveragePrice()
        {
            if (products.Count == 0)
                return 0;

            return products.Average(p => p.CalculateFinalPrice());
        }

        // Метод получения информации о магазине
        public string GetShopInfo()
        {
            return $"Название магазина: {shopName}\n" +
                   $"Количество товаров: {GetProductCount()}\n" +
                   $"Товаров со скидкой: {GetDiscountedProductCount()}\n" +
                   $"Общая стоимость: {GetTotalInventoryValue():C2}\n" +
                   $"Средняя цена: {GetAveragePrice():C2}";
        }

        // Переопределение ToString
        public override string ToString()
        {
            return $"{shopName} ({GetProductCount()} товаров)";
        }
    }
}