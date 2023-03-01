using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using WebApp.Model;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ERPContext _context;

        public HomeController(ERPContext context)
        {
            _context = context;
        }

        // GET: T_wb_Poorder
        public async Task<IActionResult> Index()
        {
            var table = new List<ShowTable>();
            var poorder =await _context.T_wb_Poorder.ToListAsync();
            var pentry = await _context.T_wb_PoorderEntry.ToListAsync();

            table = returnST(poorder, pentry);
            //foreach (var item in poorder)
            //{
            //    ShowTable t=new();
            //    t.FCustNo= item.FCustNo;
            //    t.FDate= item.FDate;
            //    t.FRemark= item.FRemark;
            //    t.FID= item.FID;
            //    t.FBillNo= item.FBillNo;
            //    t.PoorderEntriesList = pentry;
            //    table.Add(t);
            //}
            //爆超出索引
            //for (int i = 0; i < poorder.Count(); i++)
            //{

            //    Console.WriteLine(poorder[i].FCustNo);
            //    //table[i].FID = poorder[i].FID;
            //    table[i].FCustNo = poorder[i].FCustNo;
            //    //table[i].FDate = poorder[i].FDate;
            //    //table[i].FRemark = poorder[i].FRemark;
            //    //table[i].FBillNo = poorder[i].FBillNo;
            //}
            return _context.T_wb_Poorder != null ?
                        View(table) :
                        Problem("Entity set 'ERPContext.T_wb_Poorder'  is null.");
        }
        
        #region
        /// <summary>
        /// 转ShowTable
        /// </summary>
        /// <param name="poorder"></param>
        /// <param name="poorderEntry"></param>
        /// <returns></returns>
        public List<ShowTable> returnST(List<T_wb_Poorder> poorder,List<T_wb_PoorderEntry> poorderEntry)
        {
            List<ShowTable> table = new();
            foreach (var item in poorder)
            {
                ShowTable t = new();
                t.FCustNo = item.FCustNo;
                t.FDate = item.FDate;
                t.FRemark = item.FRemark;
                t.FID = item.FID;
                t.FBillNo = item.FBillNo;
                t.PoorderEntriesList = poorderEntry.Where(x =>x.FIndex==item.FID).ToList();
                table.Add(t);
            }
            return table;
        }
        private List<ShowTable> returnST(List<T_wb_Poorder> poorder)
        {
            List<ShowTable> table = new();
            foreach (var item in poorder)
            {
                ShowTable t = new();
                t.FCustNo = item.FCustNo;
                t.FDate = item.FDate;
                t.FRemark = item.FRemark;
                t.FID = item.FID;
                t.FBillNo = item.FBillNo;
                table.Add(t);
            }
            return table;
        }
        private ShowTable returnST(T_wb_Poorder poorder,List<T_wb_PoorderEntry> poorderEntry)
        {
            ShowTable t = new();
            t.FCustNo = poorder.FCustNo;
            t.FDate = poorder.FDate;
            t.FRemark = poorder.FRemark;
            t.FID = poorder.FID;
            t.FBillNo = poorder.FBillNo;
            t.PoorderEntriesList= poorderEntry;
            return t;
        }
        private ShowTable returnST(T_wb_Poorder poorder)
        {
                ShowTable t = new();
                t.FCustNo = poorder.FCustNo;
                t.FDate = poorder.FDate;
                t.FRemark = poorder.FRemark;
                t.FID = poorder.FID;
                t.FBillNo = poorder.FBillNo;
            return t;
        }
        /// <summary>
        /// 转poorder
        /// </summary>
        /// <param name="showTables"></param>
        /// <returns></returns>
        public T_wb_Poorder returnPoorder(ShowTable item)
        {
                T_wb_Poorder t = new();
                t.FCustNo = item.FCustNo;
                t.FDate = item.FDate;
                t.FRemark = item.FRemark;
                t.FID = item.FID;
                t.FBillNo = item.FBillNo;
            return t;
        }

        public List<T_wb_PoorderEntry> returnPoorderEntry(ShowTable i)
        {
            List<T_wb_PoorderEntry> t = new();
            t = i.PoorderEntriesList;
            return t;
        }
        #endregion
        // GET: T_wb_Poorder/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.T_wb_Poorder == null)
            {
                return NotFound();
            }

            var t_wb_Poorder = await _context.T_wb_Poorder
                .FirstOrDefaultAsync(m => m.FID == id);
            var t_wb_PoorderEntry = await _context.T_wb_PoorderEntry.Where(m => m.FIndex == id).ToListAsync();
            if (t_wb_Poorder == null)
            {
                return NotFound();
            }
            var showtable = returnST(t_wb_Poorder,t_wb_PoorderEntry);
            return View(showtable);
        }

        // GET: T_wb_Poorder/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FCustNo,FDate,FRemark,FID,FBillNo,PoorderEntriesList")] ShowTable showTable)
        {
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    var poorder = returnPoorder(showTable);
                    _context.Add(poorder);
                    await _context.SaveChangesAsync();
                    if (showTable.PoorderEntriesList != null)
                    {
                        T_wb_PoorderEntry pe = new();
                        foreach (var entry in showTable.PoorderEntriesList)
                        {
                            pe.FItemNo = entry.FItemNo;
                            pe.FID = entry.FID;
                            pe.FPrice = entry.FPrice;
                            pe.FAmount = entry.FAmount;
                            pe.FQty = entry.FQty;
                            pe.FJHDate = entry.FJHDate;
                            pe.FRemark = entry.FRemark;
                            pe.FIndex = poorder.FID;
                            _context.Add(pe);
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                    
                    transaction.Commit();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(showTable);
        }

        // GET: T_wb_Poorder/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.T_wb_Poorder == null)
            {
                return NotFound();
            }

            var t_wb_Poorder = await _context.T_wb_Poorder.FindAsync(id);
            var t_wb_PoorderEntry= await _context.T_wb_PoorderEntry.Where(x =>x.FIndex==id).ToListAsync();
            if (t_wb_Poorder == null)
            {
                return NotFound();
            }

            ShowTable showtable = returnST(t_wb_Poorder, t_wb_PoorderEntry);

            return View(showtable);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FCustNo,FDate,FRemark,FID,FBillNo,PoorderEntriesList")] ShowTable showTable)
        {
            if (id != showTable.FID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        var p = returnPoorder(showTable);
                        var pe = returnPoorderEntry(showTable);
                        _context.Update(p);
                        await _context.SaveChangesAsync();
                        foreach (var item in pe)
                        {
                            _context.Update(item);
                            await _context.SaveChangesAsync();
                        }
                        transaction.Commit();
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!T_wb_PoorderExists(showTable.FID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(showTable);
        }

        // GET: T_wb_Poorder/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.T_wb_Poorder == null)
            {
                return NotFound();
            }

            var t_wb_Poorder = await _context.T_wb_Poorder
                .FirstOrDefaultAsync(m => m.FID == id);
            if (t_wb_Poorder == null)
            {
                return NotFound();
            }
            var showtable = returnST(t_wb_Poorder);
            return View(showtable);
        }



        // POST: T_wb_Poorder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.T_wb_Poorder == null)
            {
                return Problem("Entity set 'ERPContext.T_wb_Poorder'  is null.");
            }
            var t_wb_Poorder = await _context.T_wb_Poorder.FindAsync(id);
            if (t_wb_Poorder != null)
            {
                _context.T_wb_Poorder.Remove(t_wb_Poorder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool T_wb_PoorderExists(int id)
        {
            return (_context.T_wb_Poorder?.Any(e => e.FID == id)).GetValueOrDefault();
        }
    }

    public class ShowTable
    {
        /// <summary>
        /// 供应商
        /// </summary>
        public string? FCustNo { get; set; }
        /// <summary>
        /// 订单日期
        /// </summary>
        public DateTime FDate { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string? FRemark { get; set; }
        /// <summary>
        /// 内码
        /// </summary>
        public int FID { get; set; }
        /// <summary>
        /// 单据编码
        /// </summary>
        public string? FBillNo { get; set; }
        /// <summary>
        /// 购买商品
        /// </summary>
        public List<T_wb_PoorderEntry>? PoorderEntriesList { get; set; }
    }
}
