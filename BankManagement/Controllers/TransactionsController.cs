using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankManagement.Models;
using BankManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BankManagement;
using System.Transactions;

namespace BankManagement.Controllers
{
    [Authorize(Roles = "Cashier, Teller, Admin")]
    public class TransactionsController : Controller

    {
        ICustomerRepository customerRepo;
        IErrorRepository errorRepo;
        IAccountRepository accountRepo;
        ITransactionsRepository transactionsRepo;
        AccountContext _context;
        public TransactionsController(ICustomerRepository customer_repo, IErrorRepository error_repo, IAccountRepository account_repo, ITransactionsRepository transactions_repo, AccountContext context)
        {
            this.customerRepo = customer_repo;
            this.errorRepo = error_repo;
            this.accountRepo = account_repo;
            this.transactionsRepo = transactions_repo;
            this._context = context;
        }

        public IActionResult ViewAll()
        {
            return View(accountRepo.ViewAll());
        }

        public IActionResult ViewDetails(Account account)
        {
            AccountDetailsViewModel advm = new AccountDetailsViewModel
            {
                account = account,
                customer = customerRepo.GetCustomerByID(account.CID)
            };

            return View(advm);
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

        public IActionResult TransferBalanceViewAll(int AID)
        {
            BalanceTransferViewModel bview = new BalanceTransferViewModel();
            bview.senderAccountID = AID;
            return View(bview);
        }

        public IActionResult TransferBalance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult TransferBalance(BalanceTransferViewModel view)
        {
            ViewData["SenderMessage"] = null;
            ViewData["ReceiverMessage"] = null;
            ViewData["NegativeBalance"] = null;
            ViewData["ZeroBalance"] = null;

            if (ModelState.IsValid)
            {
                Account sender = accountRepo.GetAccountByID(view.senderAccountID);
                Account reciever = accountRepo.GetAccountByID(view.receiverAccountID);

                if (sender == null)
                {
                    ViewData["SenderMessage"] = "Display";
                    Error error = new Error();
                    error.Description = "Sender Account ID not found in Accounts table";
                    error.PageDetail = "Error in TransferBalance.cshtml";
                    errorRepo.AddError(error);

                }
                if (reciever == null)
                {
                    Error error = new Error();
                    error.Description = "Receiver Account ID not found in Accounts table";
                    error.PageDetail = "Error in TransferBalance.cshtml";
                    errorRepo.AddError(error);
                    ViewData["ReceiverMessage"] = "Display";

                }

                if (view.moneyToSend < 0)
                {
                    Error error = new Error();
                    error.Description = "Balanced Transferred money entered is a negative value";
                    error.PageDetail = "Error in TransferBalance.cshtml";
                    errorRepo.AddError(error);
                    ViewData["NegativeBalance"] = "Display";


                }

                else if (view.moneyToSend < 0)
                    if (view.moneyToSend == 0)
                    {
                        Error error = new Error();
                        error.Description = "Balanced Transferred money entered is a negative value";
                        error.PageDetail = "Error in TransferBalance.cshtml";
                        errorRepo.AddError(error);
                        ViewData["ZeroBalance"] = "Display";


                    }

                if (sender == null || reciever == null || view.moneyToSend <= 0)
                {
                    return View();
                }

                else
                {
                    PostTransferViewModel ptView = new PostTransferViewModel
                    {
                        sender = accountRepo.GetAccountByID(view.senderAccountID),
                        receiver = accountRepo.GetAccountByID(view.receiverAccountID),
                        moneyToSend = view.moneyToSend,
                        SID = view.senderAccountID,
                        RID = view.receiverAccountID
                    };

                    return View("ConfirmTransfer", ptView);
                }
            }

            return View();
        }

        public IActionResult ConfirmTransfer(PostTransferViewModel ptView)
        {

            return View(ptView);
        }

        [HttpPost]
        public IActionResult PostTransfer(PostTransferViewModel viewModel)
        {
            ViewData["BalanceSuccessMessage"] = null;
            ViewData["BalanceFailMessage"] = null;
            ViewData["NegativeBalance"] = null;
            ViewData["ZeroBalance"] = null;
            ViewData["NotEnoughFunds"] = null;


            ViewBag.BalanceMessage = null;
            ViewBag.NegativeBalance = null;
            PostTransferViewModel view = new PostTransferViewModel
            {
                sender = accountRepo.GetAccountByID(viewModel.SID),
                receiver = accountRepo.GetAccountByID(viewModel.RID),
                moneyToSend = viewModel.moneyToSend,
                SID = viewModel.SID,
                RID = viewModel.RID
            };


            Account sender = view.sender;
            Account receiver = view.receiver;
            decimal moneyToSend = view.moneyToSend;

            if (sender.Balance >= moneyToSend && (sender.Balance - moneyToSend) >= 0 && moneyToSend > 0)
            {

                sender.Balance -= moneyToSend;
                sender.LastUpdated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString(); ;
                receiver.Balance += moneyToSend;
                receiver.LastUpdated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();
                accountRepo.Update(sender);
                accountRepo.Update(receiver);

                Transactions senderr = new Transactions();
                senderr.AID = sender.AID;
                senderr.TransactionType = "Debit";
                senderr.TransactionAmount = moneyToSend;
                senderr.Date = DateTime.Now;
                senderr.Description = "Balance transfer into another account";
                transactionsRepo.Add(senderr);

                Transactions receiverr = new Transactions();
                receiverr.AID = receiver.AID;
                receiverr.TransactionType = "Credit";
                receiverr.TransactionAmount = moneyToSend;
                receiverr.Date = DateTime.Now;
                receiverr.Description = "Money credited by a balance transfer";
                transactionsRepo.Add(receiverr);
                ViewData["BalanceSuccessMessage"] = "Display";
                return View("ConfirmTransfer", view);

            }

            else
            {
                ViewData["BalanceFailMessage"] = "Display";

                if (moneyToSend < 0)
                {
                    ViewData["NegativeBalance"] = "Display";
                }

                else if (moneyToSend < 0)
                {
                    ViewBag.NegativeBalance = "Display";
                    return View("ConfirmTransfer", view);
                }
                {
                    ViewBag.BalanceMessage = "Fail";

                    return View("ConfirmTransfer", view);
                }

            }
        }


            public IActionResult GetStatement()
            {
                TransStatementViewModel tsvm = new TransStatementViewModel
                {
                    accountList = accountRepo.ViewAll()
                };

                return View(tsvm);
            }
            [HttpPost]
            public IActionResult GetStatement(TransStatementViewModel tsvm)
            {
                DateTime date = new DateTime(1, 1, 1);
                if ((tsvm.formModel.Ntrans != 0) && (tsvm.formModel.StartDate != date) && (tsvm.formModel.EndDate != date))
                {
                    if (tsvm.formModel.StartDate > tsvm.formModel.EndDate)
                    {
                        ViewBag.Message = "DateFailure";
                    }
                    else
                    {
                        ViewBag.Message = "Success";
                        tsvm.transList = transactionsRepo.GetTransBoth(tsvm.formModel.Ntrans, tsvm.formModel.ID, tsvm.formModel.StartDate, tsvm.formModel.EndDate);

                        if (tsvm.transList.Count() == 0)
                        {
                            ViewBag.Message = "NoResults";
                            tsvm.transList = null;
                        }
                    }

                }
                else if ((tsvm.formModel.Ntrans != 0) && (tsvm.formModel.StartDate == date) && (tsvm.formModel.EndDate == date))
                {
                    ViewBag.Message = "Success";
                    tsvm.transList = transactionsRepo.GetTransByN(tsvm.formModel.Ntrans, tsvm.formModel.ID);

                    if (tsvm.transList.Count() == 0)
                    {
                        ViewBag.Message = "NoResults";
                        tsvm.transList = null;
                    }

                }
                else if ((tsvm.formModel.Ntrans == 0) && (tsvm.formModel.StartDate != date) && (tsvm.formModel.EndDate != date))
                {
                    if (tsvm.formModel.StartDate > tsvm.formModel.EndDate)
                    {
                        ViewBag.Message = "DateFailure";
                    }
                    else
                    {
                        ViewBag.Message = "Success";
                        tsvm.transList = transactionsRepo.GetTransByDate(tsvm.formModel.StartDate, tsvm.formModel.EndDate);

                        if (tsvm.transList.Count() == 0)
                        {
                            ViewBag.Message = "NoResults";
                            tsvm.transList = null;
                        }

                    }
                }
                else
                {
                    ViewBag.Message = "FormatFailure";
                }

                tsvm.accountList = accountRepo.ViewAll();
                return View(tsvm);
            }

        public IActionResult Deposit(Account account)
        {
            DepositViewModel deposit = new DepositViewModel();
            deposit.Account = accountRepo.GetAccountByID(account.AID);

            return View(deposit);
        }

        [HttpPost]
        public IActionResult Deposit(DepositViewModel depositViewModel)
        {
            depositViewModel.Account.Balance += depositViewModel.amount;
            decimal newaccountbalance = accountRepo.Deposit(depositViewModel.Account);
            TempData["UserMessage"] = $"New Account Balance for {depositViewModel.Account.AID}: $ {newaccountbalance}";
            return RedirectToAction("Display");
        }
        public IActionResult Display()
        {
            return View();
        }



        public IActionResult WithdrawBalanceViewAll(int AID)
        {
            BalanceWithdrawViewModel bview = new BalanceWithdrawViewModel();
            bview.outAccountID = AID;
            return View(bview);
        }

        public IActionResult WithdrawBalance()
        {
            return View();
        }

        [HttpPost]
        public IActionResult WithdrawBalance(BalanceWithdrawViewModel view)
        {
            ViewBag.outMessage = null;

            ViewBag.NegativeBalance = null;

            if (ModelState.IsValid)
            {
                Account outt = accountRepo.GetAccountByID(view.outAccountID);


                if (outt == null)
                {
                    ViewBag.outMessage = "Display";
                    Error error = new Error();
                    error.Description = "Account ID not found in Accounts table";
                    error.PageDetail = "Error in WithdrawBalance.cshtml";
                    errorRepo.AddError(error);
                    return View();
                }



                else if (view.moneyToout < 0)
                {
                    Error error = new Error();
                    error.Description = "Balanced Withdraw money entered is a negative value";
                    error.PageDetail = "Error in WithdrawBalance.cshtml";
                    errorRepo.AddError(error);
                    ViewBag.NegativeBalance = "Display";
                    return View();
                }
                else
                {
                    PostWithdrawViewModel ptView = new PostWithdrawViewModel
                    {
                        outt = accountRepo.GetAccountByID(view.outAccountID),

                        moneyToout = view.moneyToout,
                        SID = view.outAccountID,

                    };

                    return View("ConfirmWithdraw", ptView);
                }
            }

            return View();
        }

        public IActionResult ConfirmWithdraw(PostWithdrawViewModel ptView)
        {

            return View(ptView);
        }

        [HttpPost]
        public IActionResult PostWithdraw(PostWithdrawViewModel viewModel)
        {

            ViewBag.BalanceMessage = null;
            ViewBag.NegativeBalance = null;
            PostWithdrawViewModel view = new PostWithdrawViewModel
            {
                outt = accountRepo.GetAccountByID(viewModel.SID),

                moneyToout = viewModel.moneyToout,
                SID = viewModel.SID,

            };


            Account outt = view.outt;

            decimal moneyToout = view.moneyToout;

            if (outt.Balance >= moneyToout && (outt.Balance - moneyToout) >= 0 && moneyToout > 0)
            {
                ViewBag.BalanceMessage = "Success";

                outt.Balance -= moneyToout;
                outt.LastUpdated = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString();

                accountRepo.Update(outt);


                Transactions outs = new Transactions();
                outs.AID = outs.AID;
                outs.TransactionType = "Debit";
                outs.TransactionAmount = moneyToout;
                outs.Date = DateTime.Now;
                outs.Description = "Balance Withdraw from your account";
                transactionsRepo.Add(outs);


                return View("ConfirmWithdraw", view);

            }

            else if (moneyToout < 0)
            {
                ViewBag.NegativeBalance = "Display";
                return View("ConfirmWithdraw", view);
            }
            {
                ViewBag.BalanceMessage = "Fail";

                return View("ConfirmWithdraw", view);
            }

        }



















    }
    }


