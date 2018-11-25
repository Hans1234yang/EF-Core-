using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace web.Controllers
{
    public class Home2Controller : Controller
    {
        public MyContext _db;

        //构造函数 注入 数据上下文
        public Home2Controller(MyContext db)
        {
            _db = db;
        }


        //离线时修改 关联数据
        public  IActionResult Update2()
        {
            var province = _db.Provinces               ///省份表中 查询第一个 有城市的省份  
                          .Include(x => x.Cities)     //顺便查询关联数据
                          .First(x => x.Cities.Any());

            var city = province.Cities[0];
            city.Name+= "再修改";

            //当修改 离线数据时， 必须要用 状态版，因为不用状态版的话，会对四川省的所有城市，都修改
            //使用状态版的话，只修改对应的model1
            _db.Entry(city).State = EntityState.Modified;

            _db.SaveChanges();
            return View();
        }

        //删除关联数据
        public IActionResult Delete1()
        {
            var province1 = _db.Provinces   //查询第一个 城市不为空 的 省份
                .Include(x => x.Cities)
                .First(y => y.Cities.Any());

            var _city = province1.Cities[1];  //查询该省份 第2个城市

            _db.Cities.Remove(_city);
            _db.SaveChanges();


            return View();
        }


        //修改关联数据
        public IActionResult Update1()
        {
            var provinceInfo = _db.Provinces
                         .Include(x => x.Cities)   //查询关联数据 城市
                         .First(x => x.Cities.Any());
            //省份中 查询第一条 城市不为空的数据

            var city = provinceInfo.Cities[0];
            city.Name += "已修改";

            _db.Cities.Update(city);
            _db.SaveChanges();
            return View();
                             
        }

        

        //查询映射4
        public IActionResult QueryProjection4()
        {
            var province = _db.Provinces
                        .Select(x => new
                        {
                            x.Id,
                            x.Name,
                            x.Cities.Count,
                            Cites = x.Cities.Where(y => y.Name == "成都").ToList()

                        }).ToList();
            return View();
        }


        //查询映射 3  
        public class ProvinceInfo
        {
            public int _Id;
            public string _Name;
            public ProvinceInfo(int Id, string Name)
            {
                _Id = Id;
                _Name = Name;
            }
        }

        public List<ProvinceInfo> Query2()
        {
            var provinceInfo = _db.Provinces
                            .Select(x => new ProvinceInfo(x.Id, x.Name))
                            .ToList<ProvinceInfo>();

            return provinceInfo;
        }


        //查询映射3 调用 封装的Query2
        public IActionResult QueryProjection3()
        {
            var provinceInfo = Query();

            foreach (var item in provinceInfo)
            {
                Console.WriteLine(item.Id);
                Console.WriteLine(item.Name);
            }

            return View();
        }





        //查询映射2 调用封装的方法 Query
        public IActionResult QueryProjection2()
        {
            var queryInfo = Query();

            foreach (var item in queryInfo)
            {
                Console.WriteLine(item.Name);
                Console.WriteLine(item.Id);
            }

            return View();
        }



        //查询映射 2  封装一个方法，被其他方法调用
        public List<dynamic> Query()
        {
            var province1 = _db.Provinces
                          .Select(s => new
                          {
                              s.Id,
                              s.Name
                          }).ToList<dynamic>();

            return province1;
        }



        // 查询方法2：  查询映射1
        public IActionResult QueryProjections1()
        {
            var province1 = _db.Provinces
                          .Select(x => x.Name)      ///查询一个省份id 是 直接select 
                          .ToList();


            var province2 = _db.Provinces
                          .Select(x => new          //查询 省份id 和省份 名字， 用 匿名方法
                          {
                              x.Id,
                              x.Name
                          })
                          .ToList();

            return View();

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}