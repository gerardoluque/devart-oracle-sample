using DotConnect.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotConnect.Context
{

	public class MyDbContextDropCreateDatabaseAlways : DropCreateDatabaseAlways<MyOracleDbContext>
	{
		protected override void Seed(MyOracleDbContext context)
		{
			MyDbContextSeeder.Seed(context);
		}
	}

	public class MyDbContextDropCreateDatabaseIfModelChanges : DropCreateDatabaseIfModelChanges<MyOracleDbContext>
	{
		protected override void Seed(MyOracleDbContext context)
		{
			MyDbContextSeeder.Seed(context);
		}
	}

	public class MyDbContextCreateDatabaseIfNotExists : CreateDatabaseIfNotExists<MyOracleDbContext>
	{
		protected override void Seed(MyOracleDbContext context)
		{
			MyDbContextSeeder.Seed(context);
		}
	}

	public static class MyDbContextSeeder
	{
		public static void Seed(MyOracleDbContext context)
		{
			context.ProductCategories.Add(new ProductCategory()
			{
				CategoryName = "prose"
			});
			context.ProductCategories.Add(new ProductCategory()
			{
				CategoryName = "novel"
			});
			context.ProductCategories.Add(new ProductCategory()
			{
				CategoryName = "poem",
				ParentCategory =
			   context.ProductCategories.Local.Single(p => p.CategoryName == "novel")
			});
			context.ProductCategories.Add(new ProductCategory()
			{
				CategoryName = "fantasy",
				ParentCategory =
				context.ProductCategories.Local.Single(p => p.CategoryName == "novel")
			});

			context.Products.Add(new Product()
			{
				ProductName = "Prueba del JorgeXXX",
				Price = 100,
				OrderNumberID = 123,
				Category =
				context.ProductCategories.Local.Single(p => p.CategoryName == "novel")
			});

			context.Products.Add(new Product()
			{
				ProductName = "Shakespeare W. Shakespeare's dramatische Werke",
				Price = 78,
				OrderNumberID = 123,
				Category =
				context.ProductCategories.Local.Single(p => p.CategoryName == "prose")
			});
			context.Products.Add(new Product()
			{
				ProductName = "Plutarchus. Plutarch's moralia",
				Price = 89,
				OrderNumberID = 123,
				Category =
				context.ProductCategories.Local.Single(p => p.CategoryName == "prose")
			});
			context.Products.Add(new Product()
			{
				ProductName = "Harrison G. B. England in Shakespeare's day",
				Price = 540,
				OrderNumberID = 123,
				Category =
				context.ProductCategories.Local.Single(p => p.CategoryName == "novel")
			});
			context.Products.Add(new Product()
			{
				ProductName = "Corkett Anne. The salamander's laughter",
				Price = 5,
				OrderNumberID = 123,
				Category =
				context.ProductCategories.Local.Single(p => p.CategoryName == "poem")
			});
		}
	}
}
