using MyShop.Core.Contracts;
using MyShop.Core.Model;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyShop.Services
{ //dont forget to add reference in unity config file
    public class BasketService : IBasketService
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

        public void AddToBasket(HttpContextBase httpContext, string productId)
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

        public void RemoveFromBasket(HttpContextBase httpContext, string itemId) {
            Basket basket = GetBasket(httpContext, true);
            BasketItem item = basket.BasketItems.FirstOrDefault( i => i.Id == itemId);

            if (item != null) {
                basket.BasketItems.Remove(item);
                basketContext.Commit();
            }
        }

        public List<BasketItemViewModel> GetBasketItems(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext, false);

            if( basket != null)
            {
                // inner join linq query to get values from basket and product
                var result = (from b in basket.BasketItems 
                              join p in productContext.GetAll() on b.ProductId equals p.Id
                              select new BasketItemViewModel()
                                { 
                                  Id = b.Id,
                                  Quantity=b.Quantity,
                                  ProductName = p.Name,
                                  Image = p.Image,
                                  Price = p.Price,
                                }
                              ).ToList();
                return result;
            }
            else
            { 
                return new List<BasketItemViewModel>(); 
            }
        }

        public BasketSummaryViewModel GetBasketSummary(HttpContextBase httpContext)
        {
            Basket basket = GetBasket(httpContext,false);
            BasketSummaryViewModel summary = new BasketSummaryViewModel(0,0);
            if (basket != null)
            {
                //linQ query to select the items and add their totals

                int? basketCount = (from item in basket.BasketItems
                                    select item.Quantity).Sum();

                decimal? basketTotal = (from item in basket.BasketItems
                                        join product in productContext.GetAll() on item.ProductId equals product.Id
                                        select item.Quantity * product.Price).Sum();

                summary.BasketCount = basketCount ?? 0; //if basket count is null will return 0
                summary.BasketTotal = basketTotal ?? decimal.Zero;

                return summary;
            }
            else
            {
                return summary;
            }
        }
    }
}
