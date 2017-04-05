using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotConnect.Models
{
	[Table("Product", Schema = "BTS")]
	public class Product
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ProductID { get; set; }

		[Required]
		[MaxLength(50)]
		public string ProductName { get; set; }

		public double Price { get; set; }
		public long OrderNumberID { get; set; }

		//[Timestamp]
		public byte[] RowVersion { get; set; }


		public virtual ProductCategory Category { get; set; }
	}

	[Table("ProductCategory", Schema = "BTS")]
	public class ProductCategory
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long CategoryID { get; set; }

		[Required]
		[MaxLength(20)]
		public string CategoryName { get; set; }

		public virtual ProductCategory ParentCategory { get; set; }
		public virtual ICollection<Product> Products { get; set; }
	}
}
