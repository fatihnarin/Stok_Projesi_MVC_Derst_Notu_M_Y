using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BonusMvcStok.Models.Entity;

namespace BonusMvcStok.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun
        DbMvcStokEntities db = new DbMvcStokEntities();
        public ActionResult Index(string p)
        {
            //var urunler = db.tblurunler.Where(x=> x.durum==true).ToList();
            var urunler = from x in db.tblurunler select x;
            if(!string.IsNullOrEmpty(p))
            {
                urunler = urunler.Where(x => x.ad.Contains(p) && x.durum == true);
            }

            return View(urunler.ToList());
        }
        [HttpGet]
        public ActionResult YeniUrun()
        {
            List<SelectListItem> ktg = (from x in db.tblkategori.ToList()
                                        select new SelectListItem
                                        {
                                            Text = x.ad,
                                            Value = x.id.ToString()
                                        }).ToList();

            ViewBag.drop = ktg; //controlerdan view e taşımak için
            return View();
        }
        [HttpPost]
        public ActionResult YeniUrun(tblurunler p)
        {
            var ktgr = db.tblkategori.Where(x => x.id == p.tblkategori.id).FirstOrDefault();
            p.tblkategori = ktgr;
            db.tblurunler.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunGetir(int id)
        {
            List<SelectListItem> kat = (from x in db.tblkategori.ToList()
                                         select new SelectListItem
                                         {
                                             Text = x.ad,
                                             Value = x.id.ToString()
                                         }).ToList() ;
            var ktgr = db.tblurunler.Find(id);
            ViewBag.urunkategori = kat;
            return View("UrunGetir", ktgr);
        }
        public ActionResult UrunGuncelle(tblurunler k)
        {
            var urun = db.tblurunler.Find(k.id);
            urun.ad = k.ad;
            urun.marka = k.marka;
            urun.alisfiyat = k.alisfiyat;
            urun.satisfiyat = k.satisfiyat;
            urun.stok = k.stok;
            var ktg = db.tblkategori.Where(x => x.id == k.tblkategori.id).FirstOrDefault();
            urun.kategori = ktg.id;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult UrunSil(tblurunler p)
        {
            var urunbul = db.tblurunler.Find(p.id);
                urunbul.durum = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}