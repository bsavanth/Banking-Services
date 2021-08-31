using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManagement.Models;
using BankManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace BankManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ErrorController : Controller
    {
        ICustomerRepository customerRepo;
        IErrorRepository errorRepo;
        IAccountRepository accountRepo;
        ITransactionsRepository transactionsRepo;
        ErrorContext _context;

        public ErrorController(ICustomerRepository customer_repo, IErrorRepository error_repo, IAccountRepository account_repo, ITransactionsRepository transactions_repo, ErrorContext context)
        {
            this.customerRepo = customer_repo;
            this.errorRepo = error_repo;
            this.accountRepo = account_repo;
            this.transactionsRepo = transactions_repo;

            this._context = context;
        }
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, string searchField)
        {
            
            ViewData["CurrentSort"] = sortOrder;
            ViewData["ErrorIDSortParm"] = String.IsNullOrEmpty(sortOrder) ? "ErrorID" : "";
            ViewData["DescriptionSortParm"] = sortOrder == "Description" ? "DescriptionASC" : "Description";
            ViewData["PageDetailSortParm"] = sortOrder == "PageDetail" ? "PageDetailASC" : "PageDetail";



            if (searchString != null)
            {
              pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var errors = from e in _context.Error select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchField == "ErrorID")
                {
                    var validID = int.TryParse(searchString, out int ID);
                    errors = errors.Where(e => e.ErrorID == ID);
                }
                else if (searchField == "Description")
                {
                    errors = errors.Where(e => e.Description.Contains(searchString));
                }
                else
                {
                    errors = errors.Where(e => e.PageDetail.Contains(searchString));
                }
                    
            }

            switch (sortOrder)
            {
                case "ErrorID":
                    errors = errors.OrderBy(e => e.ErrorID);
                    break;
                case "Description":
                    errors = errors.OrderByDescending(e => e.Description);
                    break;
                case "DescriptionASC":
                    errors = errors.OrderBy(e => e.Description);
                    break;
                case "PageDetail":
                    errors = errors.OrderByDescending(e => e.PageDetail);
                    break;
                case "PageDetailASC":
                    errors = errors.OrderBy(e => e.PageDetail);
                    break;
                default:
                    errors = errors.OrderBy(s => s.ErrorID);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Error>.CreateAsync(errors.AsNoTracking(), pageNumber ?? 1, pageSize));



        }

    }
}
