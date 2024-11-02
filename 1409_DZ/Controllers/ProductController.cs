using _1409_DZ.Models;
using Microsoft.AspNetCore.Mvc;

namespace _1409_DZ.Controllers
{
    public class ProductController : Controller
    {
        private readonly SqlContext _sqlContext;
        public ProductController(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Product> products = new List<Product>();
            string query = "SELECT [Id], [Name],[Description],[Price] FROM [Products]";
            _sqlContext.ExecuteQuery(query, reader =>
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                    });
                }
            });
            return Ok(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            HttpContext.Response.ContentType = "text/html";
            return Content("""
            <form method="post" actin="../Product/Create">
            <div class="form-group">
                <label for="Name" class="form-check-label">Name:</label>
                <input type="text" name="Name" class="form-control" required>
            </div>
            <div class="form-group">
                <label for="Description" class="form-check-label">Description:</label>
                <input type="text" name="Description" class="form-control" required>
            </div>
            <div class="form-group">
                <label for="Price" class="form-check-label">Price:</label>
                <input type="number" name="Price" class="form-control" required>
            </div>
            <div class="form-group mt-3">
                <button type="submit" class="btn btn-primary">Submit</button>
            </div>
            </form>
            """);
        }

        //https://localhost:7163/product/create
        [HttpPost]
        public IActionResult Create(Product product)
        {
            string query = $"INSERT INTO [Products] ([Id],[Name],[Description],[Price]) VALUES" +
                $"('{product.Id}','{product.Name}','{product.Description}',{product.Price})";
            _sqlContext.ExecuteQuery(query, reader => { });
            return Ok(product);
        }

        [HttpGet]
        public IActionResult Search(string keyword)
        {
            List<Product> products = new List<Product>();
            string query = $"SELECT [Id],[Name],[Description],[Price] FROM [Products] WHERE [Name] LIKE '%{keyword}%'";
            _sqlContext.ExecuteQuery(query, reader =>
            {
                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                    });
                }
            });
            return Ok(products);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Product? product = GetProduct(id);
            if (product != null)
            {
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
        private Product? GetProduct(Guid id)
        {
            Product? product = null;
            string query = $"SELECT TOP 1 [Id],[Name],[Description],[Price] FROM [Products] WHERE [Id] ='{id}'";
            _sqlContext.ExecuteQuery(query, reader =>
            {
                if (reader.Read())
                {
                    product = new Product()
                    {
                        Id = reader.GetGuid(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        Price = reader.GetDecimal(3),
                    };
                }
            });
            return product;
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            Product? product = GetProduct(id);
            if (product != null)
            {
                string query = $"DELETE FROM [Products] WHERE [Id] = '{id}'";
                _sqlContext.ExecuteQuery(query, reader => { });
                return Ok(product);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
