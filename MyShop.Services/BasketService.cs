using MyShop.Core.Contracts;
using MyShop.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{
    public class BasketService
    {
        //needs to read for cookies, add reference assemblies, system.web
        IRepository<Product> productContext;
        IRepository<Basket> basketContext;

        //cookie name
        public const string BasketSessionName = "eCommerceBasket";

        public BasketService(IRepository<Product> productContext, IRepository<Basket> basketContext)
        {
            this.productContext = productContext;
            this.basketContext = basketContext;
        }

        // read cookies from http context to check for baskets
        private Basket GetBasket(HttpContextBase httpContext, bool createIfNull)
        {
            HttpCookie cookie = httpContext.Request.Cookies.Get(BasketSessionName);

            Basket basket = new Basket();
            if (cookie != null)
            {
                string basketID = cookie.Value;
                if(!string.IsNullOrEmpty(basketID))
                {
                    basket = basketContext.Find(basketID);
                }
                else
                {
                    if(createIfNull)
                    {
                        basket = CreateNewBasket(httpContext);
                    }
                }
            }
            else
            {
                if (createIfNull)
                {
                    basket = CreateNewBasket(httpContext);
                }
            }

            return basket;
        }

        private Basket CreateNewBasket(HttpContextBase httpContext)
        {
            Basket basket = new Basket();
            basketContext.Insert(basket);
            basketContext.Commit();

            //writing a cookie to the users machine
            HttpCookie cookie = new HttpCookie(BasketSessionName);
            cookie.Value = basket.Id;
            cookie.Expires = DateTime.Now.AddDays(1);
            //Send the cookie to user machine
            httpContext.Request.Cookies.Add(cookie);

            return basket;

        }

        private void AddToBasket(HttpContextBase httpContext, string productId)
        {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault(i  => i.ProductId == productId);
            if (item == null)
            {
                item = new BasketItem()
                {
                    BasketId = basket.Id,
                    ProductId = productId,
                    Quantity = 1
                };
                basket.BasketItems.Add(item);
            }
            else
            {
                item.Quantity++; 
            }
            basketContext.Commit();
        }

        private void RemoveFromBasket(HttpContextBase httpContext, string itemId) {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault( i => i.Id == itemId);

            if (item != null) {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }
    }
}
