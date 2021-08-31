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
using static BankManagement.Helper;


namespace BankManagement.Controllers
{
    [Authorize(Roles = "CustomerExecutive, AccountExecutive, Admin")]
    public class CustomerController : Controller
    {
        ICustomerRepository customerRepo;
        IErrorRepository errorRepo;
        IAccountRepository accountRepo;
        ITransactionsRepository transactionsRepo;
        CustomerContext _context;

        //---------------------------------------------------
        // Constructor for Customer Class
        //---------------------------------------------------

        public CustomerController(ICustomerRepository customer_repo, IErrorRepository error_repo, IAccountRepository account_repo, ITransactionsRepository transactions_repo, CustomerContext context)
        {
            this.customerRepo = customer_repo;
            this.errorRepo = error_repo;
            this.accountRepo = account_repo;
            this.transactionsRepo = transactions_repo;
            this._context = context;
        }


        

        public async Task<IActionResult> Index()
        {
            return View(_context.Customer.ToList());
        }



        //-----------------------------------------------------------------
        //  Displays all the customer records in the database
        //-----------------------------------------------------------------

        [HttpGet]
        public IActionResult ViewAll()

        {
            return View(customerRepo.GetCustomer());
        }

        [HttpGet]
        public IActionResult Get(int CID)
        {
            Customer customer = customerRepo.GetCustomerByID(CID);
            if (customer != null)
            {
                return View(customer);
            }
            else
            {
                Error error = new Error();
                error.Description = "No Customer Records found";
                error.PageDetail = "Error in Get.cshtml";
                errorRepo.AddError(error);
                return RedirectToAction("Index");
            }
        }

        //-----------------------------------------------------------------
        //  ADD Edit Get method for modal interface
        //-----------------------------------------------------------------

        [NoDirectAccess]
        public async Task<IActionResult> AddOrUpdate(int CID =0)
        {
           
            if (CID == 0)
            {
                return View(new Customer());
            }

            else
            {
                var customer = await _context.Customer.FindAsync(CID);
                if(customer==null)
                {
                    return NotFound();
                }
                return View(customer);
            }
        }
        

       
        //-----------------------------------------------------------------
        //  Validates the confirm update page and updates the database
        //-----------------------------------------------------------------


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrUpdate(int CID, [Bind ("CID, SSN, Name, Age, Address1, Address2, State, City")] Customer customer)
        {

            ViewData["UpdateFailMessage"] = null;
            ViewData["AddFailMessage"] = null;
            ViewData["PostUpdateServerMessage"] = null;
            ViewData["SSNMessage"] = null;

            customer.LastUpdated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
            

            if (CID == 0 && customerRepo.GetCustomerBySSN(customer.SSN)!=null )
            {
                ViewData["SSNMessage"] = "Display";
                ViewData["AddFailMessage"] = "Display";
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrUpdate", customer) });
            }

            if (ModelState.IsValid)
                {

                    if (CID == 0)
                    {
                        _context.Add(customer);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        try
                        {
                        _context.Update(customer);
                        await _context.SaveChangesAsync();
                    }
                        catch
                        {
                            return NotFound();
                        }

                    }


                    return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Customer.ToList()) });
                }

            ViewData["PostUpdateFailMessage"] = "Display";
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrUpdate", customer) });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int CID)
        {
            var customer = _context.Customer.Find(CID);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.Customer.ToList()) });
        }



        public IActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Display(Customer customer)
        {
            return View(customer);
        }


        [HttpPost]
        public IActionResult Search(FormViewModel formView)
        {
            ViewBag.CIDMessage = "DoNotDisplay";
            ViewBag.SSNMessage = "DoNotDisplay";

            //public IActionResult Search(FormViewModel formView)
            //{
            //    ViewBag.CIDMessage = "DoNotDisplay";
            //    ViewBag.SSNMessage = "DoNotDisplay";
                if (ModelState.IsValid)
                {

                    if (formView.selection == "CID")
                    {
                        Customer customer = customerRepo.GetCustomerByID(formView.ID);
                        if (customer != null)
                        {

                            return View("Display", customer);
                        }
                        else
                        {
                            ViewBag.SSNMessage = "Display";
                            ViewBag.ErrorMessage = "Display";
                            Error error = new Error();
                            error.Description = "GetCustomerByID did not return a record";
                            error.PageDetail = "Error in search.cshtml";
                            errorRepo.AddError(error);
                            return View();
                        }
                    }
                    else if (formView.selection == "SSN")
                    {
                        Customer customer = customerRepo.GetCustomerBySSN(formView.ID);
                        if (customer != null)
                        {
                            return View("Display", customer);
                        }
                        else
                        {
                            ViewBag.CIDMessage = "Display";
                            ViewBag.ErrorMessage = "Display";
                            Error error = new Error();
                            error.Description = "GetCustomerBySSN did not return a record";
                            error.PageDetail = "Error in search.cshtml";
                            errorRepo.AddError(error);
                            return View();
                        }
                    }

                }

                return View();

            }
        } } 
        
        
   

