using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEB_253502_POBORTSEVA.Domain.Entities
{
    public class Cart
    {
        /// Список объектов в корзине 
        public Dictionary<int, CartItem> CartItems { get; set; } = new();

        /// Добавить объект в корзину 
        public virtual void AddToCart(Product product)
        {
            if (CartItems.ContainsKey(product.Id))
            {
                CartItems[product.Id].Count += 1;
            }
            else
            {
                CartItems.Add(product.Id, new CartItem { Product = product, Count = 1 });
            }
        }

        /// Удалить объект из корзины 
        public virtual void RemoveItems(int id)
        {
            if (CartItems.ContainsKey(id))
            {
                CartItems.Remove(id);
            }
        }

        /// Очистить корзину 
        public virtual void ClearAll()
        {
            CartItems.Clear();
        }

        /// Количество объектов в корзине 
        public int Count { get => CartItems.Sum(item => item.Value.Count); }

        /// Общая сумма
        public double TotalPrice
        {
            get => CartItems.Sum(item => (double)item.Value.Product.Price * item.Value.Count);
        }
    }
}
