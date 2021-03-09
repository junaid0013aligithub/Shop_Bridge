using System;
using System.Collections.Generic;
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
    public class HomeController : Controller
    {
 

        public readonly string apiurl;
        public readonly IConfiguration _configuration;
        public HomeController(IConfiguration _configuration)
        {
            apiurl = _configuration["API:baseurl"];
        }


        public async Task<IActionResult> Login(LoginReq input)
        {
            HttpContext.Session.Remove("menulist");
            if (input.userName == null || input.Password == null)
            {
                return View();
            }

            try
            {
                string url = apiurl + "Home/Login";


                var json = JsonConvert.SerializeObject(input);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using (var response = await httpClient.PostAsync(url, content))
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        
                        var list = JsonConvert.DeserializeObject<LoginRes>(apiResponse);
                        if (response.IsSuccessStatusCode)
                        {
                            TempData["LoginMsg"] = list.Message;
                            TempData["RoleID"] = list.roleId;
                            HttpContext.Session.SetString("Token", list.token);
                            return RedirectToAction("Index");
                          
                        }
                        else
                        {
                            TempData["LoginFailed"] = list.Message;
                            
                            return View();
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        public IActionResult LogOut()
        {
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetComplexData<List<Menu>>("menulist") == null)
                {
                    int roleid = int.Parse(TempData["RoleID"].ToString());
                    MenuListReq input = new MenuListReq();
                    input.roleId = roleid;


                     
                    string url = apiurl + "Home/GetMenusByRoleID";


                    var json = JsonConvert.SerializeObject(input);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    using (var httpClient = new HttpClient())
                    {
                         
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        using (var response = await httpClient.PostAsync(url, content))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            
                            var list = JsonConvert.DeserializeObject<MenuListRes>(apiResponse);
                            if (response.IsSuccessStatusCode)
                            {
                                TempData["LoginMsg"] = "Login Successfull";

                                List<Menu> menulist = new List<Menu>();
                                foreach (var item in list.Menu)
                                {
                                    Menu menu = new Menu();
                                    menu.Id = item.Id;
                                    menu.MenuName = item.MenuName;
                                    menu.ParentId = item.ParentId;
                                    menu.Controller = item.Controller;
                                    menu.Action = item.Action;
                                    menulist.Add(menu);
                                }

                                HttpContext.Session.SetComplexData("menulist", menulist);
                                
                                 
                            }
                            else
                            {
                                TempData["LoginFailed"] = "Invalid Credentials";
                                return View("Login");
                            }

                        }

                    }
                }
                    return View();
                
            }
            catch (Exception ex)
            {
                TempData["LoginFailed"] = "Internal Server Error";
                return View("Login");
            }
                
            
    }

       
    }
    
}