using System.Collections.Generic;
using System.Linq;
using Model.EF;
using PagedList;
using System;
using Common;
using System.Collections;

namespace Model.DAO
{
    public class UserDAO
    {
        OnlineShopDbContext db = null;
        public UserDAO()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(User entity)
        {
            db.User.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public long InsertForFacebook(User entity)
        {
            var user = db.User.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.User.Add(entity);
                db.SaveChanges();
                return entity.ID;

            }
            else
            {
                return user.ID;
            }
        }

        public IEnumerable<User> ListUserPage(string searchString, int page, int pageSize, bool isAdmin = false)
        {
            IQueryable<User> model = db.User;
            if (!string.IsNullOrEmpty(searchString))

            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));

            }

            if (!isAdmin)
            {
                model = model.Where(x => x.GroupID != "ADMIN");
                return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
            }
            else
            {
                return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
            }
           
        }
        public User GetbyID(string userName)
        {
            return db.User.SingleOrDefault(x => x.UserName == userName);
        }
        public List<String> GetListCredentail(string userName)
        {
             var user= db.User.SingleOrDefault(x => x.UserName == userName);
            var data = (from a in db.Credentails
                        join b in db.UserGroups on a.UserGroupID equals b.ID
                        join c in db.Roles on a.RoleID equals c.ID
                        where b.ID == user.GroupID
                        select new
                        {
                            RoleID = a.RoleID,
                            UserGroupID = a.UserGroupID
                        }).AsEnumerable().Select(x => new Credentail
                        {
                            RoleID = x.RoleID,
                            UserGroupID = x.UserGroupID
                        });
            return data.Select(x => x.RoleID).ToList();
                       
        }
        public int Login(string password, string userName, int isLoginMember = 1)
        {
            var result = db.User.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                switch (isLoginMember)
                {
                    // login on page Manager
                    case 0:
                        {
                            if (result.GroupID == CommonConstants.ADMIN_GROUP || result.GroupID == CommonConstants.MOD_GROUP)
                            {
                                if (result.Status == false)
                                {
                                    return -1;
                                }
                                else
                                {
                                    if (result.Password == password)
                                    {
                                        return 1;
                                    }
                                    else
                                    {
                                        return -2;
                                    }
                                };
                            }
                            return -3;

                        }
                      //  break;
                    //login on page Home
                    case 1:
                        {
                            if (result.Status == false)
                            {
                                return -1;
                            }
                            else
                            {
                                if (result.Password == password)
                                {
                                    return 1;
                                }
                                else
                                {
                                    return -2;
                                }
                            }
                        }
                       // break;
                    default:
                        return -3;
                }

            }
        }

      

        public bool UpdateUser(User entity)
        {
            try
            {
                var user = db.User.Find(entity.ID);
                user.Name = entity.Name;
                user.Address = entity.Address;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.GroupID = entity.GroupID;
                user.Phone = entity.Phone;
                user.Email = entity.Email;
                //  user.Status = entity.Status;
                user.ModifiedBy = entity.ModifiedBy;
                user.ModifiedDate = DateTime.Now;
                db.SaveChanges();
                return true;

            }
            catch (System.Exception)
            {

                return false;
            }
        }
        public User viewDetail(int id)
        {
            return db.User.Find(id);

        }
        public bool DeleteUser(int id)
        {
            try
            {
                var user = db.User.Find(id);
                db.User.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public bool ChangStatus(long id)
        {
            var user = db.User.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool CheckUserName(string userName)
        {
            return db.User.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.User.Count(x => x.Email == email) > 0;
        }

    }
}
