using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingSoftware.Common;
using AccountingSoftware.Models;
using AccountingSoftware.Models.Entities;
using AccountingSoftware.Models.Enums;
using AccountingSoftware.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingSoftware.Controllers
{
    public class CalendarController : Controller
    {
        private readonly AccountingSoftwareContext _context;
        //private readonly Dictionary<string, string> _monthNumber = new Dictionary<string, string>()
        //{ 
        //    { "Jan", "01" },
        //    { "Feb", "02" },
        //    { "Mar", "03" },
        //    { "Apr", "04" },
        //    { "May", "05" },
        //    { "Jun", "06" },
        //    { "Jul", "07" },
        //    { "Aug", "08" },
        //    { "Sep", "09" },
        //    { "Oct", "10" },
        //    { "Nov", "11" },
        //    { "Dec", "12" },
        //};

        public CalendarController(AccountingSoftwareContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetEvents(string start, string end)
        {
            try
            {
                if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
                    return Json(false);

                int startYear = Convert.ToInt32(start.Substring(0, 4));
                int startMonth = Convert.ToInt32(start.Substring(5, 2));
                int startDay = Convert.ToInt32(start.Substring(8, 2));

                DateTime startDate = new DateTime(startYear, startMonth, startDay).AddDays(1);

                int endYear = Convert.ToInt32(end.Substring(0, 4));
                int endMonth = Convert.ToInt32(end.Substring(5, 2));
                int endDay = Convert.ToInt32(end.Substring(8, 2));

                DateTime endDate = new DateTime(endYear, endMonth, endDay).AddDays(1);

                List<Transaction> transactions = await _context.Transaction
                    .Where(x => !x.IsDelete && x.StateCodeGuid == Codes.PassedState && x.ReceiptDate >= startDate & x.ReceiptDate < endDate)
                    .ToListAsync();

                List<DateTime> dates = new List<DateTime>();
                List<EventsViewModel> events = new List<EventsViewModel>();

                foreach (var transaction in transactions)
                {
                    List<Transaction> dateTransactions = transactions
                        .Where(x => !dates.Contains(x.ReceiptDate.Date) && x.ReceiptDate.Date == transaction.ReceiptDate.Date)
                        .OrderByDescending(x => x.ReceiptDate)
                        .ToList();

                    if (dateTransactions.Count <= 0)
                        continue;

                    EventsViewModel credit = new EventsViewModel()
                    {
                        Title = "موجودی: " + dateTransactions.First().Credit.ToString("#,0") + " تومان",
                        Start = transaction.ReceiptDate.ToString("yyyy-MM-dd")
                    };

                    EventsViewModel creditor = new EventsViewModel()
                    {
                        Title = "دریافتی: " + dateTransactions.Where(x => x.TypeCodeGuid == Codes.CreditorType).Sum(x => x.Cost).ToString("#,0") + " تومان",
                        Start = transaction.ReceiptDate.ToString("yyyy-MM-dd")
                    };

                    EventsViewModel debtor = new EventsViewModel()
                    {
                        Title = "پرداختی: " + dateTransactions.Where(x => x.TypeCodeGuid == Codes.DebtorType).Sum(x => x.Cost).ToString("#,0") + " تومان",
                        Start = transaction.ReceiptDate.ToString("yyyy-MM-dd")
                    };

                    events.AddRange(new List<EventsViewModel> { credit, creditor, debtor });

                    dates.Add(transaction.ReceiptDate.Date);
                }

                return Json(events);
            }
            catch (Exception)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowCreditorDetails(string start)
        {
            try
            {
                if (string.IsNullOrEmpty(start))
                    return BadRequest();

                int startYear = Convert.ToInt32(start.Substring(0, 4));
                int startMonth = Convert.ToInt32(start.Substring(5, 2));
                int startDay = Convert.ToInt32(start.Substring(8, 2));

                DateTime startDate = new DateTime(startYear, startMonth, startDay).AddDays(1);

                List<CalendarEventDetailsViewModel> transactions = await _context.Transaction
                    .Where(x => !x.IsDelete && x.TypeCodeGuid == Codes.CreditorType && x.StateCodeGuid == Codes.PassedState && x.ReceiptDate >= startDate & x.ReceiptDate < startDate.AddDays(1))
                    .GroupBy(x => x.IsCheckTransaction)
                    .OrderBy(x => x.Key)
                    .Select(x => new CalendarEventDetailsViewModel()
                    {
                        TrasnactionType = x.Key ? "چک" : "حساب",
                        CostSum = x.Sum(x => x.Cost).ToString("#,0") + " تومان",

                    }).ToListAsync();

                return PartialView(transactions);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ShowDebtorDetails(string start)
        {
            try
            {
                if (string.IsNullOrEmpty(start))
                    return BadRequest();

                int startYear = Convert.ToInt32(start.Substring(0, 4));
                int startMonth = Convert.ToInt32(start.Substring(5, 2));
                int startDay = Convert.ToInt32(start.Substring(8, 2));

                DateTime startDate = new DateTime(startYear, startMonth, startDay).AddDays(1);

                List<CalendarEventDetailsViewModel> transactions = await _context.Transaction
                    .Where(x => !x.IsDelete && x.TypeCodeGuid == Codes.DebtorType && x.StateCodeGuid == Codes.PassedState && x.ReceiptDate >= startDate & x.ReceiptDate < startDate.AddDays(1))
                    .GroupBy(x => x.IsCheckTransaction)
                    .OrderBy(x => x.Key)
                    .Select(x => new CalendarEventDetailsViewModel()
                    {
                        TrasnactionType = x.Key ? "چک" : "حساب",
                        CostSum = x.Sum(x => x.Cost).ToString("#,0") + " تومان",

                    }).ToListAsync();

                return PartialView(transactions);
            }
            catch (Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //[HttpGet]
        //public IActionResult CreateEvent(string start, string end)
        //{
        //    TempData["start"] = start;
        //    TempData["end"] = end;

        //    return PartialView();
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest();

        //        string start = TempData["start"].ToString();
        //        string end = TempData["end"].ToString();

        //        if ((start == null || start == string.Empty) &&
        //            (end == null || end == string.Empty))
        //            return BadRequest();

        //        string startMonthName = start.Substring(4, 3);
        //        _monthNumber.TryGetValue(startMonthName, out string startMonthNum);

        //        string endMonthName = end.Substring(4, 3);
        //        _monthNumber.TryGetValue(endMonthName, out string endMonthNum);

        //        if (startMonthNum == null || endMonthNum == null)
        //            BadRequest();

        //        string startDay = start.Substring(8, 2);
        //        string startYear = start.Substring(11, 4);
        //        string startDate = startYear + '-' + startMonthNum + '-' + startDay;

        //        string endDay = end.Substring(8, 2);
        //        string endYear = end.Substring(11, 4);
        //        string endDate = endYear + '-' + endMonthNum + '-' + endDay;

        //        Calendar calendar = new Calendar()
        //        {
        //            Title = model.EventTitle,
        //            Start = startDate,
        //            End = endDate
        //        };

        //        _context.Calendar.Add(calendar);

        //        int res = await _context.SaveChangesAsync();

        //        if (Convert.ToBoolean(res))
        //        {
        //            TempData["ToasterState"] = ToasterState.Success;
        //            TempData["ToasterType"] = ToasterType.Message;
        //            TempData["ToasterMessage"] = Messages.CreateEventSuccessful;
        //        }
        //        else
        //        {
        //            TempData["ToasterState"] = ToasterState.Error;
        //            TempData["ToasterType"] = ToasterType.Message;
        //            TempData["ToasterMessage"] = Messages.CreateEventFailed;
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["ToasterState"] = ToasterState.Error;
        //        TempData["ToasterType"] = ToasterType.Message;
        //        TempData["ToasterMessage"] = Messages.CreateEventFailed;

        //        return RedirectToAction("Index");
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> DeleteEvent(Guid eventGuid)
        //{
        //    if (eventGuid == null)
        //        return BadRequest();

        //    Calendar calendar = await _context.Calendar
        //        .SingleOrDefaultAsync(x => x.CalendarGuid == eventGuid);

        //    if (calendar == null)
        //        return NotFound();

        //    DeleteViewModel model = new DeleteViewModel()
        //    {
        //        AccountGuid = calendar.CalendarGuid,
        //        Message = Messages.DeleteEventText
        //    };

        //    return PartialView(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteEvent(DeleteViewModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest();

        //        Calendar calendar = await _context.Calendar
        //            .SingleOrDefaultAsync(x => x.CalendarGuid == model.AccountGuid);

        //        if (calendar == null)
        //            NotFound();

        //        _context.Calendar.Remove(calendar);

        //        int res = await _context.SaveChangesAsync();

        //        if (Convert.ToBoolean(res))
        //        {
        //            TempData["ToasterState"] = ToasterState.Success;
        //            TempData["ToasterType"] = ToasterType.Message;
        //            TempData["ToasterMessage"] = Messages.DeleteEventSuccessful;
        //        }
        //        else
        //        {
        //            TempData["ToasterState"] = ToasterState.Error;
        //            TempData["ToasterType"] = ToasterType.Message;
        //            TempData["ToasterMessage"] = Messages.DeleteEventFailed;
        //        }

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception)
        //    {
        //        TempData["ToasterState"] = ToasterState.Error;
        //        TempData["ToasterType"] = ToasterType.Message;
        //        TempData["ToasterMessage"] = Messages.DeleteEventFailed;

        //        return RedirectToAction("Index");
        //    }
        //}
    }
}
