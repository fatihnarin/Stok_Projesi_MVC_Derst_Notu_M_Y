using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonusMvcStok.Models.Entity;
using PagedList;
using PagedList.Mvc;

namespace BonusMvcStok.Controllers
{
    public class SatısController : Controller
    {
        // GET: Satıs
        DbMvcStokEntities db = new DbMvcStokEntities();
        public ActionResult Index(int sayfa = 1)
        {
            
            var satısliste = db.tblsatislar.ToList().ToPagedList(sayfa, 3);//her sayfa 3 tane değer göstelir--View sayfasında göster
            return View(satısliste);
        }
        [HttpGet]
        public ActionResult YeniSatis()
        {
            // ürünler
            List<SelectListItem> urun = (from x in db.tblurunler.ToList()
                                        select new SelectListItem
                                        {
                                            Text = x.ad,
                                            Value = x.id.ToString()
                                        }).ToList();

            ViewBag.drop1 = urun; //controlerdan view e taşımak için

            //personel
            List<SelectListItem> per = (from x in db.tblpersonel.ToList()
                                        select new SelectListItem
                                        {
                                            Text = x.ad + " " + x.soyad,
                                            Value = x.id.ToString()
                                        }).ToList();

            ViewBag.drop2 = per; //controlerdan view e taşımak için
            //Müşteriler
            List<SelectListItem> mus = (from x in db.tblmusteri.ToList()
                                        select new SelectListItem
                                        {
                                            Text = x.ad+" "+ x.soyad,
                                            Value = x.id.ToString()
                                        }).ToList();

            ViewBag.drop3 = mus; //controlerdan view e taşımak için
            return View();
        }
        [HttpPost]
        public ActionResult YeniSatis(tblsatislar p)
        {
            var urun = db.tblurunler.Where(x => x.id == p.tblurunler.id).FirstOrDefault();
            var musteri = db.tblmusteri.Where(x => x.id == p.tblmusteri.id).FirstOrDefault();
            var personel = db.tblpersonel.Where(x => x.id == p.tblpersonel.id).FirstOrDefault();
            p.tblurunler = urun;
            p.tblmusteri = musteri;
            p.tblpersonel = personel;
            p.tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            db.tblsatislar.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
           
        }
    }
}