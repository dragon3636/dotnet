using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
namespace Model.DAO
{
    public class CategoryDAO
    {
        private OnlineShopDbContext db = null;
        public CategoryDAO()
        {
            db = new OnlineShopDbContext();
        }
        public int TotalCategory()
        {
            return db.Category.Count();
        }
       
        public Category viewDetail(long id)
        {
            return db.Category.Find(id);
        }
        public  List<Category> ListCategory()
        {
            return db.Category.Where(x => x.Status == true&&x.ParentID!=null).ToList();
        }
        public long InsertCategory(Category entity)
        {
                db.Category.Add(entity);
                db.SaveChanges();
                return entity.ID;
        }
        public IEnumerable<Category> ListAllPaging(string searchString,int pageNumber,int pageSize )
        {
            IEnumerable<Category> model = db.Category;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.MetaTitle.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(pageNumber, pageSize);
        }
        public bool Update(Category entity)
        {
            try
            {
                var category = db.Category.Find(entity.ID);
                category.Name = entity.Name;
                category.MetaTitle = entity.MetaTitle;
                category.ParentID = entity.ParentID;
                category.DisplayOrder = entity.DisplayOrder;
                category.SeoTitle = entity.SeoTitle;
                category.ModifiedDate = DateTime.Now;
                category.MetaKeywords = entity.MetaKeywords;
                category.Status = entity.Status;
                category.ShowOnHome = entity.ShowOnHome;
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
            
            

        }
        public bool DeleteCategory(int id)
        {
            try
            {
                var category = db.Category.Find(id);
                db.Category.Remove(category);
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
