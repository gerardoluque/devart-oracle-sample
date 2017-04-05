using DotConnect.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotConnect
{
	class Program
	{
		static void Main(string[] args)
		{
			Devart.Data.Oracle.OracleMonitor monitor
		  = new Devart.Data.Oracle.OracleMonitor() { IsActive = true };

			//--------------------------------------------------------------
			// You use the capability for configuring the behavior of the EF-provider:
			//Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig config =
			//	Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
			// Now, you switch off schema name generation while generating 
			// DDL scripts and DML:

			//config.Workarounds.IgnoreSchemaName = true;

			//--------------------------------------------------------------

			/*--------------------------------------------------------------
			You can set up a connection string for DbContext in different ways.
			It can be placed into the app.config (web.config) file.
			The connection string name must be identical to the DbContext descendant name.

			<add name="MyDbContext" connectionString="Data Source=ora1020;
			User Id=test;Password=test;" providerName="Devart.Data.Oracle" />

			After that, you create a context instance, while a connection string is 
			enabled automatically:
			MyDbContext context = new MyDbContext();
			---------------------------------------------------------------*/

			/*--------------------------------------------------------------
			And now it is possible to create an instance of the provider-specific connection 
			and send it to the context constructor, like we did in this application. 
			That allows us to use the StateChange connection event to change the Oracle 
			current schema on its occurrence. Thus, we can connect as one user and 
			work on a schema owned by another user.
			---------------------------------------------------------------*/
			//DbConnection con = new Devart.Data.Oracle.OracleConnection(
			//	"Data Source=ora1020;User Id=scott;Password=tiger;");
			//con.StateChange += new StateChangeEventHandler(Connection_StateChange);
			/*---------------------------------------------------------------*/

			/*--------------------------------------------------------------
			You can choose one of database initialization
			strategies or turn off initialization:
			--------------------------------------------------------------*/
			//System.Data.Entity.Database.SetInitializer
			//  <MyDbContext>(new MyDbContextDropCreateDatabaseAlways());

			/*System.Data.Entity.Database.SetInitializer
			  <MyOracleContext>(new MyDbContextCreateDatabaseIfNotExists());
			System.Data.Entity.Database.SetInitializer
			  <MyOracleContext>(new MyDbContextDropCreateDatabaseIfModelChanges());
			System.Data.Entity.Database.SetInitializer<MyOracleContext>(null);*/
			//--------------------------------------------------------------

			/*--------------------------------------------------------------
			Let's create MyDbContext and execute a database query.
			Depending on selected database initialization strategy,
			database tables can be deleted/added, and filled with source data.
			---------------------------------------------------------------*/

			//using (MyDbContext context = new MyDbContext(con))

			try
			{
				ConsoleKeyInfo option;

				do
				{

					Console.WriteLine("(1) - Product list with categories");
					Console.WriteLine("(2) - Product list");
					Console.WriteLine("(0) - Exit");
					Console.WriteLine("");
					Console.Write("Option: ");

					option = Console.ReadKey();

					Console.Clear();

					switch (option.KeyChar)
					{
						case '1':

							using (MyOracleDbContext context = new MyOracleDbContext())
							{
								var query = context.Products.Include("Category")
											   .Where(p => p.Price > 20.0)
											   .ToList();

								foreach (var product in query)
									Console.WriteLine("{0,-10} | {1,-50} | {2} | {3}",
									  product.ProductID, product.ProductName, product.Category.CategoryName, product.OrderNumberID);

								if(query.Count() <=0)
									Console.WriteLine("No existen registros...");

								Console.ReadLine();
							}

							break;

						case '2':

							using (MyOracleDbContext context = new MyOracleDbContext())
							{
								var query = context.Products
											   .Where(p => p.Price > 20.0)
											   .ToList();

								foreach (var product in query)
									Console.WriteLine("{0,-10} | {1,-50} | {2}",
									  product.ProductID, product.ProductName, product.OrderNumberID);

								if (query.Count() <= 0)
									Console.WriteLine("No existen registros...");

								Console.ReadLine();
							}

							break;
					}

					Console.Clear();

				} while (option.KeyChar != '0');
			}
			catch(Exception ex)
			{
				throw new Exception(ex.Message, ex.InnerException);
			}
		}

		// On connection opening, we change the current schema to "TEST":
		//static void Connection_StateChange(object sender, StateChangeEventArgs e)
		//{

		//	if (e.CurrentState == ConnectionState.Open)
		//	{
		//		DbConnection connection = (DbConnection)sender;
		//		connection.ChangeDatabase("TEST");
		//	}
		//}
	}
}
