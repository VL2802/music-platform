using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ML_LogicLayer.Services;
using ML_WebApp.MDA;
using SharedLibraryML;
using System.Security.Claims;
using ML_WebApp.Services;
using ML_LogicLayer;

namespace ML_WebApp.Pages
{
    public class ProfilePageModel : PageModel
    {
        private readonly UserService _userService;
        private readonly IFile _fileService;
        private readonly UserContextService _userContextService;

        [ViewData]
        public string ActionLogger { get; set; }

        [BindProperty]
        public MDAUserBase EditableUser { get; set; }

        [BindProperty]
        public MDAUserUpdatePassword PasswordUpdate { get; set; }

        public ProfilePageModel(UserService userService, IFile fileService, UserContextService userContextService)
        {
            _userService = userService;
            _fileService = fileService;
            _userContextService = userContextService;
        }
        public void OnGet()
        {
            try {
                LoadUserData();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
            }
        }

            private void LoadUserData()
            {
            try
            {
                var userId = _userContextService.GetCurrentUserId(User);
                var user = _userService.ReadUser(userId);
                IFormFile fileImage = _fileService.LoadFileAsFormFile(user.GetImage());


                EditableUser = new MDAUserBase
                {
                    Id = user.GetId(),
                    Username = user.GetUsername(),
                    Email = user.GetEmail(),
                    PhoneNumber = user.GetPhoneNumber(),
                    FileImage = fileImage
                };

                PasswordUpdate = new MDAUserUpdatePassword
                {
                    Id = user.GetId()
                };
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
                ReloadUserData();
            }
        }

            private void ReloadUserData()
            {
                var userId = _userContextService.GetCurrentUserId(User);
                var user = _userService.ReadUser(userId);

                EditableUser.Id = user.GetId();

                if (EditableUser.FileImage == null)
                {
                    EditableUser.FileImage = _fileService.LoadFileAsFormFile(user.GetImage());
                }
            }

        public IActionResult OnPostUpdateInfo()
            {
                ModelState.Clear();
                if (!TryValidateModel(EditableUser, nameof(EditableUser)))
                {
                    ReloadUserData();

                    return Page();
                }

            try
            {
                var userId = _userContextService.GetCurrentUserId(User);
                var currentUser = _userService.ReadUser(userId);
                string filePath = currentUser.GetImage();
                Role role = currentUser.GetRole();

                if (EditableUser.FileImage != null && EditableUser.FileImage.Length > 0)
                {
                    filePath = _fileService.SaveImageFile(EditableUser.FileImage);
                }

                _userService.UpdateUserInfo(
                    currentUser,
                    EditableUser.Username,
                    EditableUser.Email,
                    EditableUser.PhoneNumber,
                    filePath
                );

                ActionLogger = "Profile updated successfully";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ActionLogger = ExceptionHandler.Handle(ex);
            ReloadUserData();
                return Page();
            }
            }

            public IActionResult OnPostUpdatePassword()
            {
              
                ModelState.Clear();
                if (!TryValidateModel(PasswordUpdate, nameof(PasswordUpdate)))
                {
                ReloadUserData();

                return Page();
                }

                try
                {
                    var userId = _userContextService.GetCurrentUserId(User);
                    var currentUser = _userService.ReadUser(userId);

                if (!_userService.VerifyPassword(currentUser, PasswordUpdate.CurrentPassword))
                {
                    ModelState.AddModelError(nameof(PasswordUpdate.CurrentPassword), "Current password is incorrect");
                    return Page();
                }


                _userService.UpdateUserPassword(currentUser, PasswordUpdate.NewPassword);
                    ActionLogger = "Password updated successfully";
                    return RedirectToPage();
                }
                catch (Exception ex)
                {
                    ActionLogger = ExceptionHandler.Handle(ex);
                ReloadUserData();
                    return Page();
                }
            }
        }
    }
