using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TygaSoft.Model
{
    /// <summary>
    /// Business entity used to model items in a shopping cart
    /// </summary>
    [Serializable]
    public class CartItemInfo
    {
        // Internal member variables
        private int quantity = 1;
        private string productId;
        private string productName;
        private decimal price;
        private string imagesUrl;

        public CartItemInfo() { }

        /// <summary>
        /// Constructor with specified initial values
        /// </summary>
        /// <param name="itemId">Id of item to add to cart</param></param>
        /// <param name="name">Name of item</param>
        /// <param name="qty">Quantity to purchase</param>
        /// <param name="price">Price of item</param>
        /// <param name="type">Item type</param>	  
        /// <param name="categoryId">Parent category id</param>
        /// <param name="productId">Parent product id</param>
        /// <param name="imagesUrl">Parent product id</param>
        public CartItemInfo(string productId, string productName, int qty, decimal price, string imagesUrl)
        {
            this.productId = productId;
            this.productName = productName;
            this.quantity = qty;
            this.price = price;
            this.productId = productId;
            this.imagesUrl = imagesUrl;
        }

        // Properties
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public decimal Subtotal
        {
            get { return (decimal)(this.quantity * this.price); }
        }

        public string ProductId
        {
            get { return productId; }
        }

        public string ProductName
        {
            get { return productName; }
        }

        public decimal Price
        {
            get { return price; }
        }

        public string ImagesUrl
        {
            get
            {
                return imagesUrl;
            }
        }
    }
}
