using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeLogic;
using ShopBridgeObjects;

namespace ShopBridgeAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        public readonly Home _home;
        public HomeController(Home _home)
        {
            this._home = _home;
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginReq input)
        {
            LoginRes res = new LoginRes();
            try
            {
                res = await _home.Login(input);
                if(res.Code=="400")
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
            catch(Exception ex)
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
        public async Task<JsonResult> GetMenusByRoleID(MenuListReq input)
        {
            MenuListRes res = new MenuListRes();
            try
            {
                res = await _home.GetMenuList(input.roleId);
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