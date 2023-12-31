using BootcampEf.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BootcampEf.Controllers
{
    public class OgrenciController: Controller
    {
        private readonly DataContext _context;
        public OgrenciController(DataContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(){
             return View(await _context.Ogrenciler.ToListAsync());
        }
        public IActionResult Create(){
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ogrenci model){
            _context.Ogrenciler.Add(model);
             await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
         public async Task<IActionResult> Edit(int? id){
            if(id==null){
                return NotFound();   // 404 sayfasına yönlendirme.
            }
            //var ogr=await _context.Ogrenciler.FindAsync(id);
            // var ogr=await _context.Ogrenciler.FirstOrDefaultAsync(o => o.OgrenciId == id);
            var ogr = await _context
                                .Ogrenciler
                                .Include(o => o.KursKayitlari)    // child değerine ulaştık
                                .ThenInclude(o => o.Kurs)         // child'ın child değerine ulaştık
                                .FirstOrDefaultAsync(o => o.OgrenciId == id);
            if(ogr==null){
                  return NotFound();
            }
            return View(ogr);
         }

          [HttpPost]
          public async Task<IActionResult> Edit(int? id,Ogrenci model){
            
            if(id!=model.OgrenciId){
                return NotFound();
            }
            if(ModelState.IsValid){          // Girilen bir değerin doğru giriş yapıldı mı kontrol aşaması.
                  try{
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                  }catch(DbUpdateConcurrencyException){
                    if(!_context.Ogrenciler.Any(o => o.OgrenciId == model.OgrenciId)){   // Any metodu tüm tablo içindeki o id değeri aramakta. Var mı? yok mu? ifade başına ! ekleyerek yoksa koşulu oluşturmuş olduk.
                          return NotFound();
                    }else{
                         throw;
                    }
                  }
                     return RedirectToAction("Index");
            }
            return View(model);
         }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var ogrenci = await _context.Ogrenciler.FindAsync(id);

            if(ogrenci == null)
            {
                return NotFound();
            }

            return View(ogrenci);
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromForm]int id)
        {
            var ogrenci = await _context.Ogrenciler.FindAsync(id);
            if(ogrenci == null)
            {
                return NotFound();
            }
            _context.Ogrenciler.Remove(ogrenci);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}