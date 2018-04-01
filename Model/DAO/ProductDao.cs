using Model.EF;
using Common;
using Model.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Model.DAO
{
    public class ProductDao
    {
        OnlineShopDbContext db = null;
        public ProductDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<Product> ListAllProduct()
        {
            return  db.Product.Where(x => x.Name != null && x.Image != null).ToList();
                    
        }
        public List<Product> ListSliderProduct(int top)
        {
            return db.Product.OrderByDescending(x => x.PromotionPrice).Take(top).ToList();
        }
        public List<Product> ListNewProduct(int top)
        {
            return db.Product.Where( x=> x.PromotionPrice == null).OrderBy(x => x.CreateDate).Take(top).ToList();
        }
        public List<Product> ListFeatureProduct(int top)
        {
            return db.Product.Where(x => x.TopHot != null && x.TopHot >= DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

        public IEnumerable<Product> ListProductPage(string searchString, int page, int pageSize)
        {
            IQueryable<Product> model = db.Product;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public Product viewDetail(long id)
        {
            return db.Product.Find(id);

        }
        public long Insert(Product product)
        {
        
            if (string.IsNullOrEmpty(product.MetaTitle))
            {
                product.MetaTitle = StringHelper.ToUnsignString(product.Name);
            }
            db.Product.Add(product);
            db.SaveChanges();
            return product.ID;
        }

        public List<Product> ListRelateProducts(long productId)
        {
            var product = db.Product.Find(productId);
            return db.Product.Where(x => x.ID != product.ID && x.CategoryId == product.CategoryId).ToList();
        }

        public bool ChangStatus(long id)
        {
            var product = db.Product.Find(id);
            product.Status = !product.Status;
            db.SaveChanges();
            return product.Status;
        }

        public List<string> ListName(string keyword)
        {
            return db.Product.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }


        public bool UpdateProduct(Product entity)
        {
            try
            {
                var product = db.Product.Find(entity.ID);
                product.Name = entity.Name;
                product.Code = entity.Code;
                if (string.IsNullOrEmpty(entity.MetaTitle))
                {
                    product.MetaTitle = StringHelper.ToUnsignString(entity.Name);

                }
                else
                {
                    product.MetaTitle = entity.MetaTitle;
                }
                product.Descriptions = entity.Descriptions;
                product.Image = entity.Image;
                product.Price = entity.Price;
                product.PromotionPrice = entity.PromotionPrice;
                product.IncludedVAT = entity.IncludedVAT;
                product.Quantity = entity.Quantity;
                product.Detail = entity.Detail;
                product.Status = entity.Status;
                product.TopHot = entity.TopHot;
                product.CategoryId = entity.CategoryId;
                product.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;

            }
            catch (System.Exception)
            {

                return false;
            }
        }
        /// <summary>
        /// get list product by CategoryID
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public List<ProductViewModel> ListByCategoryid(long categoryId, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Product.Where(x => x.CategoryId == categoryId).Count();
            var model = from a in db.Product
                        join b in db.ProductCategory
   on a.CategoryId equals b.ID
                        where a.CategoryId == categoryId
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Image = a.Image,
                            Name = a.Name,
                            Price = a.Price,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CateName = b.Name,
                            CateMetaTitle = b.MetaTitle
                        };

            return model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public List<ProductViewModel> Search(string keyword, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Product.Where(x => x.Name.Contains(keyword)).Count();
            var model = from a in db.Product
                        join b in db.ProductCategory
   on a.CategoryId equals b.ID
                        where a.Name.Contains(keyword)
                        select new ProductViewModel()
                        {
                            ID = a.ID,
                            Image = a.Image,
                            Name = a.Name,
                            Price = a.Price,
                            MetaTitle = a.MetaTitle,
                            CreateDate = a.CreateDate,
                            CateName = b.Name,
                            CateMetaTitle = b.MetaTitle
                        };

            return model.OrderByDescending(x => x.CreateDate).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public Product ViewDetail(long id)
        {
            return db.Product.Find(id);
        }
        public bool DeleteProduct(int id)
        {
            try
            {
                var product = db.Product.Find(id);
                db.Product.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
