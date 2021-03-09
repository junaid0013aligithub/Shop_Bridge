using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopBridgeEntities;
using ShopBridgeObjects;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
namespace ShopBridgeLogic
{
  public  class Home
    {
        public readonly ShopBridgeContext  _context;
        public readonly  IConfiguration _configuration;

        public  Home(ShopBridgeContext _context, IConfiguration _configuration)
        {
            this._context = _context;
            this._configuration = _configuration;
        }

        public async Task<LoginRes> Login(LoginReq input)
        {
            LoginRes res = new LoginRes();
            try
            {
                var userlist = await _context.Users.Where(x => x.UserName == input.userName).FirstOrDefaultAsync();
                if (userlist.Id == 0)
                {
                    res.Code = "400";
                    res.Message = "Login Failed";
                }

                string token = Home.GenerateToken(userlist.Id, userlist.UserName, "", _configuration);
                res.roleId = userlist.RoleId;
                res.Code = "200";
                res.Message = "Login Successfull";
                res.userName = input.userName;
                res.token = token;
            }
            catch(Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;

        }



        public async Task<MenuListRes> GetMenuList(int roleid)
        {
            MenuListRes res = new MenuListRes();
            res.roleId = roleid;
            try
            {
                var menulist = await _context.Menus.Where(x => x.RoleId == roleid && x.IsActive==true).ToListAsync();
                if (menulist.Count == 0)
                {
                   
                    res.Code = "400";
                    res.Message = "No Menu List Found";
                }
                
                res.Code = "200";
                res.Message = "Menu List Found Successfully";
                List<Menu> menu = new List<Menu>();
                foreach (var item in menulist)
                {
                    Menu menu_one = new Menu();
                    menu_one.Id = item.Id;
                    menu_one.MenuName = item.MenuName;
                    menu_one.ParentId = item.ParentId;
                    menu_one.Controller = item.Controller;
                    menu_one.Action = item.Action;
                    menu.Add(menu_one);
                }


                res.Menu = menu;
            }
            catch (Exception ex)
            {
                res.Code = "500";
                res.Message = "Internal Server Error";
            }
            return res;

        }


        public  static string GenerateToken(int userid, string userName, string roledesc,IConfiguration _configuration)
        {

            try
            {
                string key = _configuration["TokenConfig:Key"];
                string tokenexpiryinminutes = _configuration["TokenConfig:TokenExpiryinMinutes"];
                string myissuer = _configuration["TokenConfig:Issuer"];
                string myaudience = _configuration["TokenConfig:Audience"];
                IdentityOptions identityoption = new IdentityOptions();
                var claims = new Claim[]
                {
new Claim(identityoption.ClaimsIdentity.UserIdClaimType,userid.ToString()),
new Claim(identityoption.ClaimsIdentity.UserNameClaimType,userName),
new Claim(identityoption.ClaimsIdentity.RoleClaimType,roledesc)
                };

                var signingkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
                var signincredentials = new SigningCredentials(signingkey, SecurityAlgorithms.HmacSha256);
                var jwt = new JwtSecurityToken(signingCredentials: signincredentials,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(int.Parse(tokenexpiryinminutes)),
                    issuer: myissuer,
                    audience: myaudience);

                string token = new JwtSecurityTokenHandler().WriteToken(jwt).ToString();
                return token;
            }
            catch (Exception ex)
            {
                return "";
            }
        }



    }





}
