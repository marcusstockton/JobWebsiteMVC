using JobWebsiteMVC.Data;
using JobWebsiteMVC.Extensions.Alerts;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobWebsiteMVC.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private IAttachmentService _attachmentService;
        private readonly ApplicationDbContext _context;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAttachmentService attachmentService,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _attachmentService = attachmentService;
            _context = context;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string LastName { get; set; }
            public string FirstName { get; set; }

            [DataType(DataType.Date), Display(Name = "Date Of Birth")]
            public DateTime? DateOfBirth { get; set; }

            public string ImageUrl { get; set; }
            // public IFormFile Avatar { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                ImageUrl = user.Attachments.Select(x => x.Location).FirstOrDefault(),
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
           
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            user.Attachments = _context.Attachments.Where(x => x.UserId == user.Id).ToList();

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            user.Attachments = _context.Attachments.Where(x => x.UserId == user.Id).ToList();
            var attachmentList = new List<Attachment>();
            if (Request.Form.Files.Any())
            {
                var file = Request.Form.Files[0];

                var attachment = await _attachmentService.SaveAvatar(file, user);
                user.Attachments.Add(attachment);
            }
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (user.LastName != Input.LastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }
            if (user.FirstName != Input.FirstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (user.DateOfBirth != Input.DateOfBirth)
            {
                user.DateOfBirth = Input.DateOfBirth;
                await _userManager.UpdateAsync(user);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            //StatusMessage = "Your profile has been updated";
            return RedirectToPage().WithSuccess("Success", "Your profile has been updated");
        }
    }
}