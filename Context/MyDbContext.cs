using Devart.Data.Oracle.Entity.Configuration;
using DotConnect.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotConnect.Context
{
	public class MyOracleDbContext : DbContext
	{
		public MyOracleDbContext() : base("MyOracleDbContext")
		{
			//Database.SetInitializer<MyOracleDbContext>(new MyDbContextCreateDatabaseIfNotExists());
			Database.SetInitializer<MyOracleDbContext>(new MyDbContextDropCreateDatabaseIfModelChanges());
			//Database.SetInitializer<MyOracleDbContext>(new MyDbContextDropCreateDatabaseAlways());


			((IObjectContextAdapter)this).ObjectContext.SavingChanges += new EventHandler(ObjectContext_SavingChanges);
		}

		//public MyDbContext(DbConnection connection) : base(connection, true) { }

		public DbSet<Product> Products { get; set; }
		public DbSet<ProductCategory> ProductCategories { get; set; }


		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{

			base.OnModelCreating(modelBuilder);

			var config = OracleEntityProviderConfig.Instance;

			config.CodeFirstOptions.TruncateLongDefaultNames = true;
			config.CodeFirstOptions.AddTableNameInDefaultIndexName = false;



			modelBuilder.Entity<Product>()
				.Property(e => e.RowVersion)
				.IsConcurrencyToken();

			//modelBuilder.Conventions.Remove();

			/*-------------------------------------------------------------
			ColumnTypeCasingConvention should be removed for dotConnect for Oracle.
			This option is obligatory only for SqlClient.
			Turning off ColumnTypeCasingConvention isn't necessary
			for  dotConnect for MySQL, PostgreSQL, and SQLite.
			-------------------------------------------------------------*/

			//modelBuilder.Conventions
			//.Remove<System.Data.Entity.ModelConfiguration.Conventions
			//.ColumnTypeCasingConvention>();

			/*-------------------------------------------------------------
			If you don't want to create and use EdmMetadata table
			for monitoring the correspondence
			between the current model and table structure
			created in a database, then turn off IncludeMetadataConvention:
			-------------------------------------------------------------*/

			//modelBuilder.Conventions
			//  .Remove<System.Data.Entity.Infrastructure.IncludeMetadataConvention>();

			/*-------------------------------------------------------------
			In the sample above we have defined autoincrement columns in the primary key
			and non-nullable columns using DataAnnotation attributes.
			Similarly, the same can be done with Fluent mapping
			-------------------------------------------------------------*/
			/*
			modelBuilder.Entity<Product>().HasKey(p => p.ProductID);
			modelBuilder.Entity<Product>().Property(p => p.ProductID)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<Product>().Property(p => p.ProductName)
				.IsRequired()
				.HasMaxLength(50);
			modelBuilder.Entity<ProductCategory>().HasKey(p => p.CategoryID);
			modelBuilder.Entity<ProductCategory>().Property(p => p.CategoryID)
				.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
			modelBuilder.Entity<ProductCategory>().Property(p => p.CategoryName)
				.IsRequired()
				.HasMaxLength(20);
			modelBuilder.Entity<Product>().ToTable("Product", "CONFIG");
			modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory", "CONFIG");
			*/
			//-------------------------------------------------------------//

			//modelBuilder.HasDefaultSchema("CONFIG");

		}

		static void ObjectContext_SavingChanges(object sender, EventArgs e)
		{

			ObjectContext context = (ObjectContext)sender;

			var addedOrModified = context.ObjectStateManager
			  .GetObjectStateEntries(EntityState.Added | EntityState.Modified)
			  .Where(entry => entry.Entity is Product)
			  .Select(entry => (Product)entry.Entity)
			  .ToList();

			foreach (Product item in addedOrModified)
			{
				item.RowVersion = Guid.NewGuid().ToByteArray();
			}

			context.DetectChanges();
		}
	}
}
