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
        public MyContext _db2;

        //构造函数注入 ，上下文
        public HomeController(MyContext db,MyContext db2)
        {
            _db = db;
            _db2 = db2;
        }


        //级联， 增加离线数据 ，必须使用外键！
        public IActionResult Insert3()
        {
            var city1 = new City
            {
                Name = "巴中市",
                AreaCode = "6666",
                ProvinceId=37   
                
            };
            _db.Cities.Add(city1);
            _db.SaveChanges();
            return View();
        }

        //在现有 省份添加数据 
        public IActionResult Insert2()
        {
            var province1 = _db.Provinces.Single(x=>x.Name=="四川");

            province1.Cities.Add(new City
            {
                Name="达州",
                AreaCode="2222"
            });

            _db.SaveChanges();

            return View();
        }


        //级联增加 省份， 城市
        public IActionResult Insert()
        {
            var province1 = new Province
            {
                Name = "四川",
                Population = 30000000,

                Cities = new List<City>
                {
                    new City{ Name="成都",AreaCode="0000"},
                    new City{ Name="攀枝花",AreaCode="1111"}
                }

            };

            _db.Provinces.Add(province1);
            _db.SaveChanges();

            return View();
        }

        //离线删除数据
        public IActionResult DeleteLiXian()
        {
            var province1 = _db.Provinces.LastOrDefault();

            _db2.Provinces.Remove(province1);
            _db2.SaveChanges();
            return View();
        }

        //删除数据
        public IActionResult Delete()
        {
            var province1 = _db.Provinces.LastOrDefault();

            _db.Remove(province1);
            _db.SaveChanges();
            return View();
        }



        //修改离线数据，

        //离线的意思，就是没有该上下文没有跟踪的数据。 比如db2 修改db1 查询出来的数据。
        public IActionResult UpateLiXian()
        {
            var province1 = _db.Provinces.FirstOrDefault();

            _db2.Provinces.Update(province1);
            _db2.SaveChanges();

            //判断2个上下文是否同一个上下文
            bool result;
            if(_db==_db2)
            {
                 result = true;
            }
            else
            {   //并不是同一个上下文
                result = false;
            }

            return View();
        }


        //编辑和添加一起
        public IActionResult AddAndEdit()
        {
            var province1 = _db.Provinces.FirstOrDefault();

            if(province1!=null)
            {
                province1.Population += 600;

                _db.Provinces.Add(new Province
                {
                    Name="重庆",
                    Population=30000000
                });

                _db.SaveChanges();
            }

            return View(); 
        }


        //修改
        public  IActionResult Update()
        {
            var province1 = _db.Provinces.FirstOrDefault();

            if(province1 != null)
            {
                province1.Population += 500;
                _db.SaveChanges();
            }

            return View();
        }


        //求最后last的数
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

        //批量增加
        public IActionResult About()
        {         
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

        //两个表数据同时插入
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
