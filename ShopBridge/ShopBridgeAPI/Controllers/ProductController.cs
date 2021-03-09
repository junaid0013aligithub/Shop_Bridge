using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeLogic;
using ShopBridgeObjects;

namespace ShopBridgeAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {

        public readonly ProductsLogic _productlogic;
        public ProductController(ProductsLogic _productlogic)
        {
            this._productlogic = _productlogic;
        }


        [HttpGet]
        public async Task<JsonResult> GetAllProducts()
        {
            GetAllProductsRes res = new GetAllProductsRes();
            try
            {
                res = await _productlogic.GetAllProductList("");
                if (res.Code == "400")
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                else
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = StatusCodes.Status500InternalServerError.ToString();
                res.Message = "Internal Server Error";
                return new JsonResult(res)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }

        [HttpPost]
        public async Task<JsonResult> GetProductByID(GetProductByIDReq input)
        {
            GetProductByIDRes res = new GetProductByIDRes();
            try
            {
                res = await _productlogic.GetProductByID(input.productId);
                if (res.Code == "400")
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                else
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = StatusCodes.Status500InternalServerError.ToString();
                res.Message = "Internal Server Error";
                return new JsonResult(res)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }

        [HttpPost]
        public async Task<JsonResult> GetProductByName(GetProductByNameReq input)
        {
            GetAllProductsRes res = new GetAllProductsRes();
            try
            {
                res = await _productlogic.GetAllProductList(input.productName);
                if (res.Code == "400")
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                else
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = StatusCodes.Status500InternalServerError.ToString();
                res.Message = "Internal Server Error";
                return new JsonResult(res)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }



        [HttpPost]
        public async Task<JsonResult> InsertProduct(InsertProductReq input)
        {
            DeleteProductByIDRes res = new DeleteProductByIDRes();
            try
            {
                res = await _productlogic.InsertProduct(input);
                if (res.Code == "400")
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                else
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = StatusCodes.Status500InternalServerError.ToString();
                res.Message = "Internal Server Error";
                return new JsonResult(res)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }
        [HttpPost]
        public async Task<JsonResult> UpdateProduct(UpdateProductReq input)
        {
            DeleteProductByIDRes res = new DeleteProductByIDRes();
            try
            {
                res = await _productlogic.UpdateProduct(input);
                if (res.Code == "400")
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                else
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = StatusCodes.Status500InternalServerError.ToString();
                res.Message = "Internal Server Error";
                return new JsonResult(res)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }
        [HttpPost]
        public async Task<JsonResult> DeleteProduct(DeleteProductByIDReq input)
        {
            DeleteProductByIDRes res = new DeleteProductByIDRes();
            try
            {
                res = await _productlogic.DeleteProduct(input.productId);
                if (res.Code == "400")
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                }
                else
                {
                    return new JsonResult(res)
                    {
                        StatusCode = StatusCodes.Status200OK
                    };
                }
            }
            catch (Exception ex)
            {
                res.Code = StatusCodes.Status500InternalServerError.ToString();
                res.Message = "Internal Server Error";
                return new JsonResult(res)
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }


        }









    }
}