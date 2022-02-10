using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcFirmaCagri.Models.Entity;
namespace MvcFirmaCagri.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        DbisTakipEntities db = new DbisTakipEntities();

       
        public ActionResult AktifCagrilar()
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(p=>p.Durum==true && p.CagriFirma==id).ToList();
            var cagriSayisi = db.TblCagrilar.Where(x => x.Durum == true && x.CagriFirma == id).Count();
            if (cagriSayisi == 0)
            {
                ViewBag.CagriSayisi = "Aktif Çağrınız bulunmamaktadır.";
            }
            return View(cagrilar);
        }
        public ActionResult PasifCagrilar()
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(p => p.Durum == false && p.CagriFirma == id).ToList();
            return View(cagrilar);
        }
        [HttpGet]
        public ActionResult YeniCagri()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniCagri(TblCagrilar p)
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            p.Durum = true;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.CagriFirma = id;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }
        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetay.Where(x => x.Cagri == id).ToList();
            return View(cagri);
        }
        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilar.Find(id);
            return View("CagriGetir",cagri);
        }
        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilar.Find(p.ID);
            cagri.Konu = p.Konu;
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }
        [HttpGet]
        public ActionResult ProfilDuzenle()
        {
            return View();
            //var mail = (String)Session["Mail"];
            //var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            //var profil = db.TblFirmalar.Where(x => x.ID == id).FirstOrDefault();
            //return View( profil);
        }
        [HttpPost]
        public ActionResult ProfilDuzenle(TblFirmalar p)
        {
           
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var data=db.TblFirmalar.Find(id);
            data.Ad = p.Ad;
            data.Adres = p.Adres;
            data.Yetkili = p.Yetkili;
            data.Telefon = p.Telefon;
            data.il = p.il;
            data.ilce = p.ilce;
            data.Sektor = p.Sektor;
            data.Sifre = p.Sifre;           
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
            //var profil = db.TblFirmalar.Where(x => x.ID == id).FirstOrDefault();
            //return View( profil);
        }
        public ActionResult Anasayfa()
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var toplamcagri = db.TblCagrilar.Where(x => x.CagriFirma == id).Count();
            var aktifcagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            var pasifcagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            var yetkili = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Yetkili).FirstOrDefault();
            var sektor = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Sektor).FirstOrDefault();
            var firmaadi = db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Ad).FirstOrDefault();
            var firmagorsel= db.TblFirmalar.Where(x => x.ID == id).Select(y => y.Gorsel).FirstOrDefault();
            ViewBag.c1 = toplamcagri;
            ViewBag.c2 = aktifcagri;
            ViewBag.c3 = pasifcagri;
            ViewBag.c4 = yetkili;
            ViewBag.c5 = sektor;
            ViewBag.c6 = firmaadi;
            ViewBag.c7 = firmagorsel;
            return View();
        }
        public PartialViewResult Partial1()
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var mesajlar = db.TblMesajlar.Where(x => x.Alici == id &&  x.Durum==true).ToList();
            var mesajsayisi = db.TblMesajlar.Where(x => x.Alici == id && x.Durum == true).Count();
            ViewBag.m1 = mesajsayisi;
            return PartialView(mesajlar);
        }
        public PartialViewResult Partial2()
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum==true).ToList();
            var cagrisayisi = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            ViewBag.m1 = cagrisayisi;
            return PartialView(cagrilar);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }
        public PartialViewResult Partial3()
        {
                
            return PartialView();
        }

        public ActionResult GonderilenMesajlar(TblFirmalar p)
        {

            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var data = db.TblMesajlar.Where(c => c.Gonderen == id && c.Durum==true).ToList();
            return View(data);
            //var profil = db.TblFirmalar.Where(x => x.ID == id).FirstOrDefault();
            
        }
        public ActionResult GelenMesajlar(TblFirmalar p)
        {

            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            var data = db.TblMesajlar.Where(c => c.Alici == id && c.Durum==true).ToList();
            return View(data);
            //var profil = db.TblFirmalar.Where(x => x.ID == id).FirstOrDefault();

        }
        [HttpGet]
        public ActionResult YeniMesaj()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YeniMesaj(TblMesajlar p)
        {
            var mail = (String)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.ID).FirstOrDefault();
            p.Durum = true;
            p.Gonderen = id;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());          
            db.TblMesajlar.Add(p);
            db.SaveChanges();
            return RedirectToAction("GonderilenMesajlar");
        }

    }
}