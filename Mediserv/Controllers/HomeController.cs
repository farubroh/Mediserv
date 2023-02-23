using Mediserv.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.IO;
using System.Net.NetworkInformation;
using PagedList;
using System.Web.UI;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;


namespace Mediserv.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private  MediservEntities db = new MediservEntities();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Login(Users log)
        {
            Users content = db.Users.Where(x => x.UserName == log.UserName && x.Password == log.Password).FirstOrDefault();

            if (content != null && content.UserName=="admin" && content.Password=="1234")
            {
                Session["ad_id"] = content.UserId.ToString();
                return RedirectToAction("Admin_dashboard");
            }
            else if(content != null)
            {
                var customer=db.Users.Where(x => x.UserName==log.UserName && x.Password==log.Password).Count();
                var id = db.Users.Where(x => x.UserName == log.UserName && x.Password == log.Password).Select   (v=>v.UserId).FirstOrDefault();
                Session["uid"]=id;

                Session["ID"] = customer;
                return RedirectToAction("Index");
            }
            else
            {
                Response.Write("<script>alert('Invalid username or password')</script>");
                ViewBag.error = "Invalid username or password";
                return View();
            }
            return View();
        }
        public ActionResult Index()
        {
           
                List<Categories> ind = db.Categories.ToList();
                ViewBag.shoplist = ind;
                return View();
            
            
            
            
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Book()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }





        public ActionResult Registration()
        {

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Registration(Users reg)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (ModelState.IsValid)
            {
                Users cat = new Users();
                cat.UserName= reg.UserName;
                cat.Password= reg.Password;
                cat.Email= reg.Email;
                cat.RPassword= reg.RPassword;
                db.Users.Add(cat);
                db.SaveChanges();

               
                return RedirectToAction("Login");
                //return View();
            }
            return View();





        }

        public ActionResult Admin_Dashboard()
        {
            return View();
        }
        public ActionResult Admin_Dashboards()
        {
            return View();
        }
        public ActionResult Admin_Form_Controls()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin_Form_Controls(Categories cvm)
        {
            cvm.img.SaveAs(Server.MapPath("~/cat_pic/ " + cvm.img.FileName));
            cvm.ImageUrl = "~/cat_pic/ " + cvm.img.FileName;
            db.Categories.Add(cvm);
            db.SaveChanges();
            //string path = UploadImage(img);
            //Categories cat = new Categories();
            //if (path.Equals("-1"))
            //{
            //    ViewBag.error = "Image could not be uploaded.";
            //}
            //else
            //if(img!-null)
            //{
            //db.Categories.Add(cat);
            //cvm.ImageUrl = new byte[img.ContentLength];
            //img.InputStream.Read(cvm.ImageUrl,0, img.ContentLength);

            // cat.Name = cvm.Name;
            //// cat.ImageUrl = path;
            // cat.isActive = cvm.isActive;
            // cat.Address = cvm.Address;
            // cat.CountProducts = cvm.CountProducts;
            // cat.Shopcode = cvm.Shopcode;
            // cat.Description = cvm.Description;
            // cat.Shortdescription = cvm.Shortdescription;

            // db.Categories.Add(cvm);
            // db.SaveChanges();
            // ViewBag.msg = "data load";

            return View();

            //}  

            //return View();
        }

        public ActionResult Admin_Form_Control()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin_Form_Control(Categories cvm)
        {
            //cvm.img.SaveAs(Server.MapPath("~/cat_pic/ " + cvm.img.FileName));
            //cvm.ImageUrl = "~/cat_pic/ " + cvm.img.FileName;
            //db.Categories.Add(cvm);
            //db.SaveChanges();

            cvm.img.SaveAs(Server.MapPath("~/cat_pic/ " + cvm.img.FileName));
            cvm.ImageUrl = "~/cat_pic/ " + cvm.img.FileName;
            db.Categories.Add(cvm);
            db.SaveChanges();
            //string path = UploadImage(img);
            //Categories cat = new Categories();
            //if (path.Equals("-1"))
            //{
            //    ViewBag.error = "Image could not be uploaded.";
            //}
            //else
            //if (img! - null)
            //{
            //    db.Categories.Add(cat);
            //    cvm.ImageUrl = new byte[img.ContentLength];
            //    img.InputStream.Read(cvm.ImageUrl, 0, img.ContentLength);

            //    cat.Name = cvm.Name;
            //    // cat.ImageUrl = path;
            //    cat.isActive = cvm.isActive;
            //    cat.Address = cvm.Address;
            //    cat.CountProducts = cvm.CountProducts;
            //    cat.Shopcode = cvm.Shopcode;
            //    cat.Description = cvm.Description;
            //    cat.Shortdescription = cvm.Shortdescription;

            //    db.Categories.Add(cvm);
            //    db.SaveChanges();
            //    ViewBag.msg = "data load";

            //    return View();

            //}

            return View();
        }

        public string UploadImage(HttpPostedFileBase img)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (img != null && img.ContentLength > 0)
            {
                string extension = Path.GetExtension(img.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload/"), random + Path.GetFileName(img.FileName));
                        img.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(img.FileName);
                       
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }

                }
                else
                {
                    Response.Write("<script>alert('Only jpg,jpeg or png ara available');</script>");
                   
                }


            }
            else
            {
                Response.Write("<script>alert('please select a file');</script>");
                path= "-1";
               
            }
            return path;
        }

        public ActionResult View_catagory(int? page)
        {
            int pagesize = 4, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.Categories.Where(x => x.isActive == "active").OrderByDescending(x => x.CategoryId).ToList();
            IPagedList<Categories> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        public ActionResult add_products()
        {
            List<Categories> stu = db.Categories.ToList();
            ViewBag.shoplist = new SelectList(stu, "CategoryId", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult add_products(Products p)
        {
            Products cat = new Products();
            List<Categories> stu = db.Categories.ToList();
            ViewBag.shoplist = new SelectList(stu, "CategoryId", "Name");

            cat.Name = p.Name;
            cat.Description = p.Description;
            cat.Price = p.Price;
            cat.Qunatity = p.Qunatity;
            cat.IsActive = p.IsActive;
            cat.CategoryId = p.CategoryId;
            //cat.pro_fk_user = (int?)Session["UserId"];
            db.Products.Add(cat);
            db.SaveChanges();
            return View();
        }

        public ActionResult view_products_index(int ?id)
        {
            Products p = new Products();
          
            if (id == null)
            {
                //p.ProductId = db.Products.ToList();
            }
            else
            {
                List<Products> ind = db.Products.ToList();
                //List<Products> ind = db.Products.ToList();
               ind = db.Products.Where(x => x.CategoryId == id).ToList();
                ViewBag.shoplist = ind;
                return View();
            }
            return View();

        }
        public ActionResult Shops()
        {
            List<Categories> ind = db.Categories.ToList();
            ViewBag.shoplist = ind;
           
            return View();

        }
        public ActionResult addtocart(int id)
        {
            List<Products> list;
            if (Session["myCart"] == null)
            {
                list = new List<Products>();
            }
            else
            {
                list = (List<Products>)Session["myCart"];
            }
            
            list.Add(db.Products.Where(p => p.ProductId == id).FirstOrDefault());
            list[list.Count - 1].p_in_cart = 1;
            Session["myCart"] = list;
            return RedirectToAction("view_cart");
            //return View();
        }

        public ActionResult view_cart()
        {
           
            return View();

        }

        public ActionResult MinustoCart(int Rowno)
        {
            List<Products> list = (List<Products>)Session["myCart"];
            list[Rowno].p_in_cart--;
            Session["myCart"] = list;
            return RedirectToAction("view_cart");
            //return View();
        }

        public ActionResult PlustoCart(int Rowno)
        {
            List<Products> list = (List<Products>)Session["myCart"];
            list[Rowno].p_in_cart++;
            Session["myCart"] = list;
            return RedirectToAction("view_cart");
            //return View();
        }

        public ActionResult RemoveFromCart(int Rowno)
        {
            List<Products> list = (List<Products>)Session["myCart"];
            list.RemoveAt(Rowno);
            Session["myCart"] = list;
            return RedirectToAction("view_cart");
            //return View();
        }

        public ActionResult Blog_details()
        {
            
            return View();
        }

        public ActionResult Contacts()
        {

            return View();
        }
        //[HttpPost]
        //public ActionResult view_cart(Products p)
        //{

        //    List<Products> list = (List<Products>)Session["myCart"];
        //    list[Rowno].p_in_cart--;
        //    Session["myCart"] = list;
        //    return View(); List<Products> list;
        //    if (Session["myCart"] == null)
        //    {
        //        list = new List<Products>();
        //    }
        //    else
        //    {
        //        list = (List<Products>)Session["myCart"];
        //    }

        //    list.Add(db.Products.Where(p => p.ProductId == id).FirstOrDefault());
        //    list[list.Count - 1].p_in_cart = 1;
        //    Session["myCart"] = list;
        //    return RedirectToAction("view_cart");

        //}
        public ActionResult Paynow()
        {

            if (Session["ID"] != null)
            {
                Orders o = new Orders();
                o.order_status = "paid";
                o.order_date = DateTime.Now;
                o.u_id =(int) Session["uid"];
                Session["Order"]=o;
                db.Orders.Add(o);
                db.SaveChanges();
                //return Redirect("https://www.sandbox.paypal.com/cgi-bin/websrc?cmd=_xclick&business=ranazubair321@gmail.com&item_name=Mediserv&return=https://localhost:44383/Home/view_cart/OrderBooked&amount=" + double.Parse(Session["totalamount"].ToString())/120);
                return View();
            }
            else
            {
                return Redirect("Login");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["ID"] = null;
            return RedirectToAction("Index","Home");
        }

        public ActionResult OrderBooked()
        {
            return View();
        }





    }
} 