using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EFCore.Data;
using EFCore.DomainModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using web.Models;

namespace web.Controllers
{

    public class HomeController : Controller
    {
        public MyContext _db;

        //构造函数注入 上下文
        public HomeController(MyContext db)
        {
            _db = db;
        }


        //求最后的数
        public IActionResult LastOrDefault()
        {
            var MyPara = "北%";
            var province = _db.Provinces
                         .Where(x => EF.Functions.Like(x.Name, MyPara))
                         .LastOrDefault();

            return View();
        }

        /// <summary>
        /// 模糊查询的 第2种方法 
        /// </summary>
        /// <returns></returns>
        public IActionResult Like()
        {
            var MyParam = "北%";
            
            var province = _db.Provinces
                         .Where( x => EF.Functions.Like(x.Name, MyParam))
                         .ToList();

            return View();
        }

        //contain 模糊查询  第1种方法
        public IActionResult Contains()
        {
            var param = "北";
            var province = _db.Provinces
                          .Where(x => x.Name.Contains(param))
                          .ToList();
            return View();
        }

        //非lambda表达式的 find方法 查询
        public IActionResult Find()
        {
            var ParamId = 3;

            var province = _db.Provinces.Find(ParamId);

            return View();
        }

        //firstOrDefault 也能执行查询
        public IActionResult FirstOrDefault()
        {
            var Myparam = "北京";
            var province = _db.Provinces
                           .Where(x => x.Name == Myparam)
                           .FirstOrDefault();


            return View();
        }

        //参数化使用 lambda 表达式 
        public IActionResult SelectLambda()
        {
            var param = "北京";

            var province1 = _db.Provinces
                           .Where(x => x.Name == param)
                           .ToList();

            return View();
        }


        public IActionResult Select()
        {
            var provinces1 = _db.Provinces
                   .Where(x => x.Name == "北京")
                  .ToList();



            var provinces2 = (from p in _db.Provinces
                              where p.Name == "北京"
                              select p).ToList();
            return View();
        }

        public IActionResult Index()
        {
            var Province1 = new Province
            {
                Name = "北京",
                Population = 20000000
            };

            _db.Provinces.Add(Province1);
            //上下文根据 province实体


            _db.SaveChanges();

            return View();
        }

        public IActionResult About()
        {
            //批量增加
            var province1 = new Province
            {
                Name = "北京",
                Population = 20000000

            };

            var province2 = new Province
            {
                Name = "上海",
                Population = 10000000
            };

            var province3 = new Province
            {
                Name = "广东",
                Population = 80000000
            };

            //批量添加方法 1
            _db.Provinces.AddRange(province1, province2, province3);


            //批量添加方法 2
            //_db.Provinces.AddRange(new List<Province>
            //{
            //    province1,province2,province3

            //});

            _db.SaveChanges();

            return View();
        }

        public IActionResult Contact()
        {
            var province1 = new Province
            {
                Name = "天津",
                Population = 8000000
            };

            var company1 = new Company
            {
                Name = "渣打it",
                LegalPerson = "李开复"
            };

            //执行不同的表操作时，不需要db.具体表。  而是db.addrange直接一点
            _db.AddRange(province1, company1);

            _db.SaveChanges();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
