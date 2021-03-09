using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShopBridgeObjects;

namespace ShopBridgeWeb.Controllers
{
    public class ProductController : Controller
    {
        public readonly string apiurl;
        public readonly IConfiguration _configuration;
        public ProductController(IConfiguration _configuration)
        {
            apiurl = _configuration["API:baseurl"];
        }

        public IActionResult Index()
        {
            try
            {

            }
            catch (Exception ex)
            {
                //Logger.LogError(ex.ToString());
            }
            return View();
        }

        public async Task<IActionResult> AddProduct(Product product, IFormFile Photo)
        {

          
                int productid = 0;
                try
            {
                if (product.Name == null && product.Description == null)
                {
                    ViewData["Title"] = "ADD PRODUCT";
                    return View();
                }

                if (Photo != null)
                {


                    if (Photo.Length > 0)
                    {
                        using (var target = new MemoryStream())
                        {
                            Photo.CopyTo(target);
                            product.Photo = target.ToArray();
                        }
                    }

                }
                   
                if (TempData["ProductId"] != null)
                {
                    productid = int.Parse(TempData["ProductId"].ToString());
                    if (productid != product.Id)
                    {
                        ViewBag.UserInsert = "InValid Product ID";
                        return View("Index");
                    }
                    TempData.Keep("Id");
                }

                if (productid == 0)
                {
                    ViewData["Title"] = "ADD PRODUCT";
                    string url = apiurl+"Product/InsertProduct";

                    var json = JsonConvert.SerializeObject(product);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        string token= HttpContext.Session.GetString("Token");
                        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PostAsync(url, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var msg = JsonConvert.DeserializeObject<DeleteProductByIDRes>(apiResponse);
                           
                            if (response.IsSuccessStatusCode)
                            {
                                ViewBag.InsertUpdate = msg.Message;
                                return View("Index");
                            }
                            else
                            {
                                ViewBag.InsertUpdate = msg.Message;
                                return View("Index");
                            }

                        }
                    }

                }
                else
                {
                    if(product.Price==null)
                    {
                        product.Price = 0;
                    }

                    ViewData["Title"] = "UPDATE PRODUCT";

                    string url = apiurl + "Product/UpdateProduct";
                    var json = JsonConvert.SerializeObject(product);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                        string token = HttpContext.Session.GetString("Token");
                        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PostAsync(url, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            var rolelist = JsonConvert.DeserializeObject<DeleteProductByIDRes>(apiResponse);
                            
                            if (response.IsSuccessStatusCode)
                            {
                                ViewBag.InsertUpdate = rolelist.Message;
                            }
                            else
                            {
                                ViewBag.InsertUpdate = rolelist.Message; 
                            }
                            TempData.Remove("Id");
                        }

                    }

                }




            }
            catch (Exception ex)
            {
                 
            }

            return View("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(string productId)
        {
            try
            {
               
                if (int.Parse(productId) == 0)
                {
                    ViewBag.Delete = "Product ID Required";
                    return View("Index");
                }
                 
                string url = apiurl + "Product/DeleteProduct"; 
                DeleteProductByIDReq deletereq = new DeleteProductByIDReq();
                deletereq.productId = int.Parse(productId);
                var json = JsonConvert.SerializeObject(deletereq);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    string token = HttpContext.Session.GetString("Token");
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        var rolelist = JsonConvert.DeserializeObject<DeleteProductByIDRes>(apiResponse);
                        if (response.IsSuccessStatusCode)
                        {
                            ViewBag.Delete = rolelist.Message;
                        }
                        else
                        {
                            ViewBag.Delete = rolelist.Message;
                           
                        }

                        return Json(rolelist);
                    }

                }

            }
            catch (Exception ex)
            {
                return View("Index");
            }
            
        }




        [HttpPost]
        public async Task<JsonResult> GetAllProducts(string productName)
        {
            try
            {
                string url = "";

                GetAllProductsRes list = new GetAllProductsRes();
                if (productName == null)
                {
                    url = apiurl + "Product/GetAllProducts";


                    using (var httpClient = new HttpClient())
                    {
                        string token = HttpContext.Session.GetString("Token");

                        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                        httpClient.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer",token);



                        using (var response = await httpClient.GetAsync(url))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                list = JsonConvert.DeserializeObject<GetAllProductsRes>(apiResponse);

                            }
                        }

                    }
                }
                else
                {
                    url = apiurl + "Product/GetProductByName";

                    GetProductByNameReq req = new GetProductByNameReq();
                    req.productName = productName;
                    var json = JsonConvert.SerializeObject(req);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        string token = HttpContext.Session.GetString("Token");
                        var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                        httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        using (var response = await httpClient.PostAsync(url, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                list = JsonConvert.DeserializeObject<GetAllProductsRes>(apiResponse);

                            }
                        }

                    }

                }
                List<Product> products = new List<Product>();
                
                foreach (var item in list.Product)
                {
                    Product product = new Product();

                    product.Id = item.Id; 
                    product.Name = item.Name; 
                    product.Price = item.Price; 
                    product.Description = item.Description;
                     product.Image = item.Image; 
                    products.Add(product);
                }

                

                return Json(products);
                 
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpGet]
        public async Task<IActionResult> GetProductByID(int productId)
        {
            try
            {
                string url = "";

                url = apiurl + "Product/GetProductByID";

                GetProductByIDRes list = new GetProductByIDRes();
                GetProductByIDReq deletereq = new GetProductByIDReq();
                deletereq.productId = productId;
                var json = JsonConvert.SerializeObject(deletereq);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    string token = HttpContext.Session.GetString("Token");
                    var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                    httpClient.DefaultRequestHeaders.Accept.Add(contentType);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        if (response.IsSuccessStatusCode)
                        {
                            list = JsonConvert.DeserializeObject<GetProductByIDRes>(apiResponse);

                        }
                    }

                }
                
                    Product product = new Product();

                    product.Id = list.Product.Id;
                    product.Name = list.Product.Name;
                    product.Price = list.Product.Price;
                    product.Description = list.Product.Description;
                    product.Image = list.Product.Image;

                TempData["ProductId"] = product.Id;

                ViewData["Title"] = "UPDATE PRODUCT";
                ViewBag.ProductID = product.Id;
                ViewBag.ImageURL = product.Image;
                return View("AddProduct",product);

            }
            catch (Exception ex)
            {
                return View("Index");
            }

        }




    }
}