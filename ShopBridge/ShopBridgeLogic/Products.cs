using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShopBridgeEntities;
using ShopBridgeObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopBridgeLogic
{
   public class ProductsLogic
    {

        public readonly ShopBridgeContext _context;
        public readonly IHostingEnvironment hostingEnv;
        public readonly IHttpContextAccessor httpContextAccessor;
        public readonly IConfiguration _configuration;

        public ProductsLogic(ShopBridgeContext _context,IHostingEnvironment hostingEnv,
            IHttpContextAccessor httpContextAccessor,IConfiguration _configuration)
        {
            this._context = _context;
            this.hostingEnv = hostingEnv;
            this.httpContextAccessor = httpContextAccessor;
            this._configuration = _configuration;
        }

        public async Task<GetAllProductsRes> GetAllProductList(string productname)
        {
            GetAllProductsRes res = new GetAllProductsRes();
            dynamic productlist;
            try
            {
                if (productname == "")
                {
                     productlist = await _context.Products.ToListAsync();
                }
                else
                {
                     productlist = await _context.Products.Where(x=>x.Name.ToLower().Contains(productname)).ToListAsync();
                }
                if(productlist.Count==0)
                {
                    res.Code = "400";
                    res.Message = "No Product Found";

                }
                else
                {
                    res.Code = "200";
                    res.Message = "Product List Found Successfully";
                    List<Product> products = new List<Product>();
                    foreach (var item in productlist)
                    {
                        Product product = new Product();
                        product.Price = item.Price;
                        product.Description = item.Description;
                        product.Name = item.Name;
                       // product.Image = item.Image;
                        product.Id = item.Id;
                        product.Image = GetImageURL(item.Id, hostingEnv, httpContextAccessor, _configuration);
                        products.Add(product);
                    }
                    res.Product = products;
                }
            }
            catch(Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;
        }




        public async Task<GetProductByIDRes> GetProductByID(int productid)
        {
            GetProductByIDRes res = new GetProductByIDRes();
            try
            {
                var productlist = await _context.Products.Where(x=>x.Id==productid).FirstOrDefaultAsync();
                if (productlist == null)
                {
                    res.Code = "400";
                    res.Message = "No Product Found";

                }
                else
                {
                    res.Code = "200";
                    res.Message = "Product List Found Successfully";
                    
                        Product product = new Product();
                        product.Price = productlist.Price;
                        product.Description = productlist.Description;
                        product.Name = productlist.Name;
                    // product.Image = productlist.Image;
                    product.Image = GetImageURL(productlist.Id, hostingEnv, httpContextAccessor, _configuration);
                    product.Id = productlist.Id;
                        res.Product = product;
                }
            }
            catch (Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;
        }



        public async Task<DeleteProductByIDRes> InsertProduct(InsertProductReq input)
        {
            DeleteProductByIDRes res = new DeleteProductByIDRes();
            try
            {
                Products product = new Products();
                product.Name = input.Name;
                product.Price = input.Price;
                product.Description = input.Description;
                product.Image =  Convert.ToBase64String(input.Photo);
                product.CreatedOn = DateTime.Now;
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                SaveImage(product.Id, input.Photo, hostingEnv, _configuration);
                   res.productId = product.Id;
                    res.Code = "200";
                    res.Message = "Product Inserted Successfully";
                
            }
            catch (Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;
        }

        public async Task<DeleteProductByIDRes> UpdateProduct(UpdateProductReq input)
        {
            DeleteProductByIDRes res = new DeleteProductByIDRes();
            try
            {
                var productlist = await _context.Products.Where(x => x.Id == input.Id).FirstOrDefaultAsync();
                if (productlist == null)
                {
                    res.Code = "400";
                    res.Message = "No Product Found";

                }
                else
                {
                    Products product = new Products();
                    if (input.Photo != null)// && input.Photo.Trim() != "")
                    {
                        productlist.Image = Convert.ToBase64String(input.Photo);
                    }
                    if (input.Name != null && input.Name.Trim() != "")
                    {
                        productlist.Name = input.Name;
                    }
                    if (input.Description != null && input.Description.Trim() != "")
                    {
                        productlist.Description = input.Description;
                    }
                    if (input.Price != 0 )
                    {
                        productlist.Price = input.Price;
                    }
                    productlist.UpdatedOn = DateTime.Now;
                    _context.Entry(productlist).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    if(input.Photo!=null)
                    {
                        SaveImage(input.Id, input.Photo, hostingEnv, _configuration);
                    }
                    res.productId = input.Id;
                    res.Code = "200";
                    res.Message = "Product Updated Successfully Successfully";

                    
                }
            }
            catch (Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;
        }

        public async Task<DeleteProductByIDRes> DeleteProduct(int productid)
        {
            DeleteProductByIDRes res = new DeleteProductByIDRes();
            try
            {
                var productlist = await _context.Products.Where(x => x.Id == productid ).FirstOrDefaultAsync();
                if (productlist == null)
                {
                    res.Code = "400";
                    res.Message = "No Product Found";

                }
                else
                {
                    _context.Products.Remove(productlist);
                  await  _context.SaveChangesAsync();
                    res.productId = productid;
                    res.Code = "200";
                    res.Message = "Product Deleted Successfully";
                    
                }
            }
            catch (Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;
        }

        public string GetImageURL(int productid, IHostingEnvironment hostingEnv,
            IHttpContextAccessor httpContextAccessor, IConfiguration _configuration)
        {
            string webRootPath = hostingEnv.WebRootPath;
            string contentRootPath = hostingEnv.ContentRootPath;
            string imagefolder = _configuration["ProductImage:FolderName"];
            string imagetype = _configuration["ProductImage:ImageType"];
            string folderpath = contentRootPath + "\\" + imagefolder;

            string host = httpContextAccessor.HttpContext.Request.Host.Value;
            string scheme = httpContextAccessor.HttpContext.Request.Scheme;
            int? port = httpContextAccessor.HttpContext.Request.Host.Port;

            string url = scheme + "://" + host + "/" + imagefolder + "/" + productid.ToString() + imagetype;
            return url;


        }


        public bool SaveImage(int productid, byte[] photo, IHostingEnvironment hostingEnv,
                                 IConfiguration _configuration)
        {
            try
            {

                string webRootPath = hostingEnv.WebRootPath;
                string contentRootPath = hostingEnv.ContentRootPath;
                string imagefolder = _configuration["ProductImage:FolderName"];
                string imagetype = _configuration["ProductImage:ImageType"];
                string folderpath = contentRootPath + "\\" + imagefolder;




                bool retvalue = true;
                string photostring = Convert.ToBase64String(photo);
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(photostring), 0, Convert.FromBase64String(photostring).Length);
                var myfilename = productid;// item.productTitle;

                if (!Directory.Exists(folderpath))
                {
                    Directory.CreateDirectory(folderpath);
                }
                string filepath = folderpath + "\\" + myfilename + imagetype;
                using (var imageFile = new FileStream(filepath, FileMode.Create))
                {
                    imageFile.Write(Convert.FromBase64String(photostring), 0, Convert.FromBase64String(photostring).Length);
                    imageFile.Flush();
                }

                return retvalue;
            }
            catch(Exception ex)
            {
                return false;
            }
        }



    }
}
