using EFCore.DomainModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace EFCore.Data
{
    public class MyContext:DbContext
    {
        public MyContext(DbContextOptions<MyContext> options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //cityid 和companyid 作联合主键  

            //fluent api 方式配置
           

            modelBuilder.Entity<City>().HasOne(x => x.Province).WithMany(x => x.Cities)
                                   .HasForeignKey(x=>x.ProvinceId);

            modelBuilder.Entity<CityCompany>()   //这个中间表 有2个联合主键
                .HasKey(x=>new { x.CityId,x.CompanyId});

            modelBuilder.Entity<CityCompany>()  //一个城市对应多个CityCompany
                .HasOne(x=>x.City).WithMany(x=>x.CityCompanies).HasForeignKey(x=>x.CityId);

            modelBuilder.Entity<CityCompany>()   //一个公司对应多个CityCompnay
                .HasOne(x => x.Company).WithMany(x => x.CityCompanies).HasForeignKey(x=>x.CompanyId);

            modelBuilder.Entity<Mayor>()  //市长和 城市是一对一的关系
                .HasOne(x=>x.City).WithOne(x=>x.Mayor).HasForeignKey<Mayor>(x=>x.CityId);
        }

        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CityCompany> CityCompanies { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Mayor> Mayors { get; set; }

    }
}
