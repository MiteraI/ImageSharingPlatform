using ImageSharingPlatform.Domain.Entities;
using ImageSharingPlatform.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace ImageSharingPlatform.Pages.Profile
{
    public class TransactionModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITransactionService _transactionService;

        public TransactionModel(IUserService userService, ITransactionService transactionService)
        {
			_userService = userService;
			_transactionService = transactionService;
		}

        [BindProperty]
        public IList<Transaction> Transactions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var loggedInUser = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(loggedInUser))
            {
				return Redirect("/Authentication/Login");
			}

            Transactions = (List<Transaction>) await _transactionService.GetAllForUser(JsonConvert.DeserializeObject<User>(loggedInUser).Id);

            return Page();
        }
    }
}
