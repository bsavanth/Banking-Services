using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManagement.Models;
using BankManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using BankManagement;
using Microsoft.EntityFrameworkCore;

namespace BankManagement.Controllers
{
    [Authorize(Roles = "CustomerExecutive, AccountExecutive, Admin")]
    public class AccountController : Controller
    {
        ICustomerRepository customerRepo;
        IErrorRepository errorRepo;
        IAccountRepository accountRepo;
        ITransactionsRepository transactionsRepo;
        AccountContext _context;
        CustomerContext cusContext;
        

        public AccountController(ICustomerRepository customer_repo, IErrorRepository error_repo, IAccountRepository account_repo, ITransactionsRepository transactions_repo, AccountContext context, CustomerContext cuscontext)
        {
            this.customerRepo = customer_repo;
            this.errorRepo = error_repo;
            this.accountRepo = account_repo;
            this.transactionsRepo = transactions_repo;
            this._context = context;
            this.cusContext = cuscontext;

        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber, string searchField)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["AIDSortParm"] = String.IsNullOrEmpty(sortOrder) ? "AID" : "";
            ViewData["CIDSortParm"] = sortOrder == "CID" ? "CIDASC" : "CID";
            ViewData["AccountTypeSortParm"] = sortOrder == "AccountType" ? "AccountTypeASC" : "AccountType";
            ViewData["BalanceSortParm"] = sortOrder == "Balance" ? "BalanceASC" : "Balance";
            ViewData["LastUpdatedSortParm"] = sortOrder == "LastUpdatedType" ? "LastUpdatedASC" : "LastUpdatedType";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var accounts = from e in _context.Account select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchField == "AID")
                {
                    var isNumeric = int.TryParse(searchString, out int ID);
                    accounts = accounts.Where(a => a.AID == ID);
                }
                else if (searchField == "CID")
                {
                    var isNumeric = int.TryParse(searchString, out int ID);
                    accounts = accounts.Where(a => a.CID == ID);
                }
                else if (searchField == "AccountType")
                {
                    accounts = accounts.Where(a => a.AccountType.Contains(searchString));
                }
                else if (searchField == "LastUpdated")
                {
                    var isNumeric = DateTime.TryParse(searchString, out DateTime date);
                    accounts = accounts.Where(a => a.LastUpdated == date.ToShortDateString());
                }
                else
                {
                    var isNumeric = decimal.TryParse(searchString, out decimal balance);
                    accounts = accounts.Where(a => a.Balance == balance);
                }
                    
            }


            switch (sortOrder)
            {
                case "AID":
                    accounts = accounts.OrderBy(e => e.AID);
                    break;
                
                case "CID":
                    accounts = accounts.OrderByDescending(e => e.CID);
                    break;
                case "CIDASC":
                    accounts = accounts.OrderBy(e => e.CID);
                    break;
                case "AccountType":
                    accounts = accounts.OrderByDescending(e => e.AccountType);
                    break;
                case "AccountTypeASC":
                    accounts = accounts.OrderBy(e => e.AccountType);
                    break;
                case "Balance":
                    accounts = accounts.OrderByDescending(e => e.Balance);
                    break;
                case "BalanceASC":
                    accounts = accounts.OrderBy(e => e.Balance);
                    break;
                case "LastUpdated":
                    accounts = accounts.OrderByDescending(e => e.LastUpdated);
                    break;
                case "LastUpdatedASC":
                    accounts = accounts.OrderBy(e => e.LastUpdated);
                    break;
                default:
                    accounts = accounts.OrderBy(e => e.AID);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Account>.CreateAsync(accounts.AsNoTracking(), pageNumber ?? 1, pageSize));



        }

        public IActionResult ViewAll()
        {
            return View(accountRepo.ViewAll());
        }

        [HttpGet]
        public IActionResult ViewDetails(Account account)
        {
            AccountDetailsViewModel advm = new AccountDetailsViewModel
            {
                account = account,
                customer = customerRepo.GetCustomerByID(account.CID)
            };

            return View(advm);
        }

        public async Task<IActionResult> customerPagination(string sortOrder, string currentFilter, string searchString, int? pageNumber, string searchField)
        {

            ViewData["CurrentSort"] = sortOrder;
            ViewData["CIDSortParm"] = String.IsNullOrEmpty(sortOrder) ? "CID" : "";
            ViewData["SSNSortParm"] = sortOrder == "SSN" ? "SSNASC" : "SSN";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "NameASC" : "Name";
            ViewData["AgeSortParm"] = sortOrder == "Age" ? "AgeASC" : "Age";
            ViewData["Address1SortParm"] = sortOrder == "Address1" ? "Address1ASC" : "Address1";
            ViewData["Address2SortParm"] = sortOrder == "Address2" ? "Address2ASC" : "Address2";
            ViewData["CitySortParm"] = sortOrder == "City" ? "CityASC" : "City";
            ViewData["StateSortParm"] = sortOrder == "State" ? "StateASC" : "State";
            ViewData["LastUpdatedSortParm"] = sortOrder == "LastUpdatedType" ? "LastUpdatedASC" : "LastUpdatedType";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var customers = from e in cusContext.Customer select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchField == "CID")
                {
                    var isNumeric = int.TryParse(searchString, out int ID);
                    customers = customers.Where(a => a.CID == ID);
                }
                else if (searchField == "SSN")
                {
                    var isNumeric = int.TryParse(searchString, out int ID);
                    customers = customers.Where(a => a.SSN == ID);
                }
                else if (searchField == "Name")
                {
                    customers = customers.Where(a => a.Name.Contains(searchString));
                }
                else if (searchField == "Age")
                {
                    var isNumeric = int.TryParse(searchString, out int ID);
                    customers = customers.Where(a => a.Age == ID);
                }
                else if (searchField == "Address1")
                {
                    customers = customers.Where(a => a.Address1.Contains(searchString));
                }
                else if (searchField == "Address2")
                {
                    customers = customers.Where(a => a.Address2.Contains(searchString));
                }
                else if (searchField == "City")
                {
                    customers = customers.Where(a => a.City.Contains(searchString));
                }
                else if (searchField == "State")
                {
                    customers = customers.Where(a => a.State.Contains(searchString));
                }
                else
                {
                    var isNumeric = DateTime.TryParse(searchString, out DateTime date);
                    customers = customers.Where(a => a.LastUpdated == date.ToShortDateString());
                }

            }


            switch (sortOrder)
            {
                case "CID":
                    customers = customers.OrderBy(e => e.CID);
                    break;

                case "SSN":
                    customers = customers.OrderByDescending(e => e.SSN);
                    break;
                case "SSNASC":
                    customers = customers.OrderBy(e => e.SSN);
                    break;
                case "Name":
                    customers = customers.OrderByDescending(e => e.Name);
                    break;
                case "NameASC":
                    customers = customers.OrderBy(e => e.Name);
                    break;
                case "Age":
                    customers = customers.OrderByDescending(e => e.Age);
                    break;
                case "AgeASC":
                    customers = customers.OrderBy(e => e.Age);
                    break;
                case "Address1":
                    customers = customers.OrderByDescending(e => e.Address1);
                    break;
                case "Address1ASC":
                    customers = customers.OrderBy(e => e.Address1);
                    break;
                case "Address2":
                    customers = customers.OrderByDescending(e => e.Address2);
                    break;
                case "Address2ASC":
                    customers = customers.OrderBy(e => e.Address2);
                    break;
                case "City":
                    customers = customers.OrderByDescending(e => e.City);
                    break;
                case "CityASC":
                    customers = customers.OrderBy(e => e.City);
                    break;
                case "State":
                    customers = customers.OrderByDescending(e => e.State);
                    break;
                case "StateASC":
                    customers = customers.OrderBy(e => e.State);
                    break;
                case "LastUpdated":
                    customers = customers.OrderByDescending(e => e.LastUpdated);
                    break;
                case "LastUpdatedASC":
                    customers = customers.OrderBy(e => e.LastUpdated);
                    break;
                default:
                    customers = customers.OrderBy(e => e.CID);
                    break;
            }

            int pageSize = 10;
            return View(await PaginatedList<Customer>.CreateAsync(customers.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        public IActionResult AddFromViewAll(int CID)
        {
            
            AddDropDownForm form = new AddDropDownForm();
            form.account = new Account();
            form.account.CID = CID;
            return View(form);
        }

       

        public IActionResult AddView(AddDropDownForm from)
        {
            AddDropDownForm tempform = new AddDropDownForm();
            tempform.customers = customerRepo.GetCustomer(); 
            return View(tempform);
        }
        [HttpPost]
        public IActionResult Add(AddDropDownForm form)
        {

            ViewData["Control"] = null;
            ViewData["AddSuccessMessage"] = null;
            ViewData["AddFailMessage"] = null;
            ViewData["AddCIDMessage"] = null;
            ViewData["AddBalanceMessage"] = null;
            ViewData["AddDuplicate"] = null;
            ViewData["AccountType"] = form.account.AccountType.ToString();

            AddDropDownForm tempform = new AddDropDownForm();
            tempform.customers = customerRepo.GetCustomer();
            tempform.account = new Account();
            tempform.account.CID = form.account.CID;
            tempform.account.AccountType = form.account.AccountType;
            tempform.account.Balance = form.account.Balance;

            if (ModelState.IsValid)
            {
                form.account.LastUpdated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

                if(accountRepo.DuplicateAccount(form.account.CID, form.account.AccountType) == true)
                {
                    ViewData["AddDuplicate"] = "Display";
                    if (form.account.Balance < 0)
                    {
                        ViewData["AddBalanceMessage"] = "Display";
                        
                    }
                    return View("AddView", tempform);
                }

                if (form.account.Balance < 0)
                {
                    ViewData["AddBalanceMessage"] = "Display";
                    if (accountRepo.DuplicateAccount(form.account.CID, form.account.AccountType) == true)
                    {
                        ViewData["AddDuplicate"] = "Display";
                    }
                    return View("AddView", tempform);
                }


                 int val = accountRepo.Add(form.account);
                 if (val == 1 )
                 {
                        ViewData["AddSuccessMessage"] = "Display";
                        return View("AddView", tempform);
                 }
                else
                {
                    Error error = new Error();
                    error.Description = "Add account was not successful";
                    error.PageDetail = "Error in add.cshtml";
                    errorRepo.AddError(error);
                    ViewData["AddFailMessage"] = "Display";
                    return View("AddView", tempform);
                }
            }

            
            return View("AddView", tempform);
            


        }


        //Delete

        public IActionResult Delete()
        {


            return View();

        }


        [HttpPost]
        public IActionResult Delete(FormViewModel formView)
        {
            ViewBag.CIDMessage = "DoNotDisplay";
            ViewBag.SSNMessage = "DoNotDisplay";
            ViewBag.ErrorMessage = "DoNotDisplay";
            ViewBag.DeleteSuccess = null;


            if (ModelState.IsValid)
            {


                if (formView.selection == "CID")
                {
                    Account account = accountRepo.GetAccountsByCustomer(formView.ID);
                    if (account != null)
                    {

                        return View("ConfirmDelete", account);
                    }
                    else
                    {
                        ViewBag.SSNMessage = "Display";
                        ViewBag.ErrorMessage = "Display";
                        Error error = new Error();
                        error.Description = "GetCustomerByID did not return a record";
                        error.PageDetail = "Error in delete.cshtml";
                        errorRepo.AddError(error);
                        return View();
                    }
                }
                else if (formView.selection == "SSN")
                {

                    Account account = accountRepo.GetAccountsByCustomer(formView.ID);
                    if (account != null)
                    {
                        return View("ConfirmDelete", account);
                    }
                    else
                    {
                        ViewBag.CIDMessage = "Display";
                        ViewBag.ErrorMessage = "Display";
                        Error error = new Error();
                        error.Description = "GetCustomerBySSN did not return a record";
                        error.PageDetail = "Error in delete.cshtml";
                        errorRepo.AddError(error);
                        return View();
                    }
                }

                else
                {
                    ViewBag.SSNMessage = "Display";
                    ViewBag.CIDMessage = "Display";
                    Error error = new Error();
                    error.Description = "Unknown Error with Delete";
                    error.PageDetail = "Error in delete.cshtml";
                    errorRepo.AddError(error);
                    return View();
                }
            }

            return View();

        }

        [HttpGet]
        public IActionResult ConfirmDeslete(Account account)
        {
            if (account.Balance > 0)
            {
                ViewBag.DeleteMessage = "Display";
                return View();
            }


            return View(account);
        }
        public IActionResult Search()
        {
            return View();
        }
        public IActionResult Display(Account account)
        {
            return View(account);
        }
        [HttpPost]
        public IActionResult Search(FormViewModel formView)
        {
            ViewBag.CIDMessage = "DoNotDisplay";
            ViewBag.AIDMessage = "DoNotDisplay";



            if (ModelState.IsValid)
            {

                if (formView.selection == "CID")
                {
                    Account account = accountRepo.GetAccountByID(formView.ID);
                    if (account != null)
                    {

                        return View("Display", account);
                    }

                    else
                    {
                        ViewBag.AIDMessage = "Display";
                        ViewBag.ErrorMessage = "Display";
                        Error error = new Error();
                        error.Description = "GetAccountByID did not return a record";
                        error.PageDetail = "Error in search.cshtml";
                        errorRepo.AddError(error);
                        return View();
                    }
                }
                else if (formView.selection == "AID")
                {
                    Account account = accountRepo.GetAccountByID(formView.ID);
                    if (account != null)
                    {
                        return View("Display", account);
                    }
                    else
                    {
                        ViewBag.CIDMessage = "Display";
                        ViewBag.ErrorMessage = "Display";
                        Error error = new Error();
                        error.Description = "GetAccountByID did not return a record";
                        error.PageDetail = "Error in search.cshtml";
                        errorRepo.AddError(error);
                        return View();
                    }
                }

                else
                {
                    ViewBag.AIDMessage = "Display";
                    ViewBag.CIDMessage = "Display";
                    Error error = new Error();
                    errorRepo.AddError(error);
                    return View();
                }
            }

            return View();

        }
    }
}
