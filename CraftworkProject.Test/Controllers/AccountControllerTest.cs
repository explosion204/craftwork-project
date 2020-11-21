using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CraftworkProject.Domain;
using CraftworkProject.Domain.Models;
using CraftworkProject.Services.Interfaces;
using CraftworkProject.Test.Utils;
using CraftworkProject.Web.Controllers;
using CraftworkProject.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using Xunit;

namespace CraftworkProject.Test.Controllers
{
    public class AccountControllerTest
    {
        private AccountController GetControllerWithAuthenticatedUser()
        {
            var userManagerMock = new Mock<IUserManager>();
            var emailServiceMock = new Mock<IEmailService>();
            var userManagerHelperMock = new Mock<IUserManagerHelper>();

            return new AccountController(userManagerMock.Object, userManagerHelperMock.Object, emailServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new GenericIdentity("test"))
                    }
                },
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object
            };
        }

        private AccountController GetControllerWithNotAuthenticatedUser(
            bool loginSuccess = false,
            bool externalLoginSuccess = false,
            bool findsById = true,
            bool findsByUsername = false,
            bool findsByEmail = false,
            bool confirmableEmail = true,
            bool resetablePassword = true,
            bool nullExternalLoginInfo = true
        )
        {
            var testUser = DomainTestUtil.GetTestUsers(1)[0];

            var userManagerMock = new Mock<IUserManager>();
            userManagerMock.Setup(x => x.SignIn(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((a, b) => Task.FromResult(loginSuccess));
            userManagerMock.Setup(x => x.FindUserById(It.IsAny<Guid>()))
                .Returns<Guid>(a => Task.FromResult(findsById ? testUser : null));
            userManagerMock.Setup(x => x.FindUserByName(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(findsByUsername ? testUser : null));
            userManagerMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
                .Returns<string>(a => Task.FromResult(findsByEmail ? testUser : null));
            userManagerMock.Setup(x => x.CreateUser(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .Returns<User, string, Guid>((a, b, c) => Task.FromResult(Guid.NewGuid()));
            userManagerMock.Setup(x => x.GenerateEmailConfirmationToken(It.IsAny<Guid>()))
                .Returns<Guid>(a => Task.FromResult(string.Empty));
            userManagerMock.Setup(x => x.ConfirmEmail(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns<Guid, string>((a, b) => Task.FromResult(confirmableEmail));

            userManagerMock.Setup(x => x.ResetPassword(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns<Guid, string, string>((a, b, c) => Task.FromResult(resetablePassword));
            
            var emailServiceMock = new Mock<IEmailService>();
            var identityMock = new Mock<GenericIdentity>("test");
            identityMock.SetupGet(x => x.IsAuthenticated).Returns(false);
            
            var urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("callbackUrl");
            
            var externalLoginInfoMock = new Mock<ExternalLoginInfo>(new ClaimsPrincipal(), "test", "test", "test");
            var authSchemeMock = new Mock<AuthenticationScheme>("test", "test", typeof(IAuthenticationHandler));
            
            var userManagerHelperMock = new Mock<IUserManagerHelper>();
            userManagerHelperMock.Setup(x => x.GetExternalLoginInfo())
                .Returns(Task.FromResult(nullExternalLoginInfo ? null : externalLoginInfoMock.Object));
            userManagerHelperMock.Setup(x => x.AddExternalLogin(It.IsAny<Guid>(), It.IsAny<ExternalLoginInfo>()))
                .Returns<Guid, ExternalLoginInfo>((a, b) => Task.FromResult(0));
            userManagerHelperMock.Setup(x => x.GetExternalAuthenticationSchemes())
                .Returns(Task.FromResult(new List<AuthenticationScheme> {authSchemeMock.Object}));
            userManagerHelperMock.Setup(x =>
                x.ConfigureExternalAuthenticationProperties(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((a, b) => new AuthenticationProperties { RedirectUri = "test" });
            userManagerHelperMock.Setup(x => x.ExternalLoginSignIn(It.IsAny<ExternalLoginInfo>()))
                .Returns(Task.FromResult(externalLoginSuccess));

            return new AccountController(userManagerMock.Object, userManagerHelperMock.Object, emailServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(identityMock.Object)
                    }
                },
                ObjectValidator = ControllerTestUtil.GetObjectModelValidatorMock().Object,
                Url = urlHelperMock.Object
            };
        }

        [Fact]
        public void LoginAuthenticatedGetTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.Login("test_url");
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void LoginNotAuthenticatedGetTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Login("test_url");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<LoginViewModel>(viewResult.Model);
            Assert.NotNull(controller.ViewData["returnUrl"]);
        }

        [Fact]
        public void LoginAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.Login(new LoginViewModel(), "").Result;
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void LoginNotAuthenticatedPostSignInSuccessTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(true);

            var result = controller.Login(new LoginViewModel(), "test_url").Result;
            Assert.Equal("test_url", Assert.IsType<RedirectResult>(result).Url);

            result = controller.Login(new LoginViewModel(), null).Result;
            Assert.Equal("/", Assert.IsType<RedirectResult>(result).Url);
        }

        [Fact]
        public void LoginNotAuthenticatedPostSignInFailTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();
            
            var result = controller.Login(new LoginViewModel(), "test_url").Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<LoginViewModel>(viewResult.Model);
        }

        [Fact]
        public void LogoutNotAuthenticatedTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Logout();
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void LogoutAuthenticatedTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.Logout();
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void SignupAuthenticatedGetTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.Signup("test_url");
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void SignupNotAuthenticatedGetTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Signup("test_url");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SignUpViewModel>(viewResult.Model);
            Assert.NotNull(controller.ViewData["returnUrl"]);
        }

        [Fact]
        public void SignupAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.Signup(new SignUpViewModel(), "test_url").Result;
            Assert.IsType<RedirectResult>(result);
        }

        [Fact]
        public void SignupNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Signup(new SignUpViewModel
            {
                Username = "test_username", 
                Password = "test_password",
                ConfirmPassword = "test_password",
                Email = "test_email",
                ConfirmEmail = "test_email"
            }, "test_url")
            .Result;
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("test_email", viewResult.ViewData["Email"]);
            Assert.Equal("EmailSent", viewResult.ViewName);
        }
        
        [Fact]
        public void SignupNotAuthenticatedPostExternalSignupTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(nullExternalLoginInfo: false, findsById: true);
            
            var result = controller.Signup(new SignUpViewModel
                {
                    Username = "test_username", 
                    Password = "test_password",
                    ConfirmPassword = "test_password",
                    Email = "test_email",
                    ConfirmEmail = "test_email"
                }, "test_url")
                .Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/profile", redirectResult.Url);
        }

        [Fact]
        public void SignupNotAuthenticatedPostDuplicateUsernameTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(findsByUsername: true);

            var result = controller.Signup(new SignUpViewModel
                {
                    Username = "test_username", 
                    Password = "test_password",
                    ConfirmPassword = "test_password",
                    Email = "test_email",
                    ConfirmEmail = "test_email"
                }, "test_url")
                .Result;
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SignUpViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public void SignupNotAuthenticatedPostDuplicateEmailTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(findsByEmail: true);

            var result = controller.Signup(new SignUpViewModel
                {
                    Username = "test_username", 
                    Password = "test_password",
                    ConfirmPassword = "test_password",
                    Email = "test_email",
                    ConfirmEmail = "test_email"
                }, "test_url")
                .Result;
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SignUpViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }
        
        [Fact]
        public void SignupNotAuthenticatedPostPasswordsDoNotMatchTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Signup(new SignUpViewModel
                {
                    Username = "test_username", 
                    Password = "test_password",
                    ConfirmPassword = "test_password_",
                    Email = "test_email",
                    ConfirmEmail = "test_email"
                }, "test_url")
                .Result;
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SignUpViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }
        
        [Fact]
        public void SignupNotAuthenticatedPostEmailsDoNotMatchTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.Signup(new SignUpViewModel
                {
                    Username = "test_username", 
                    Password = "test_password",
                    ConfirmPassword = "test_password",
                    Email = "test_email",
                    ConfirmEmail = "test_email_"
                }, "test_url")
                .Result;
            
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SignUpViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public void ExternalSignupAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ExternalSignUp("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalSignupNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.ExternalSignUp("test").Result;
            var challengeResult = Assert.IsType<ChallengeResult>(result);
            Assert.Equal("test", challengeResult.Properties.RedirectUri);
        }
        
        [Fact]
        public void ExternalLoginAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ExternalLogin("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalLoginNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.ExternalLogin("test").Result;
            var challengeResult = Assert.IsType<ChallengeResult>(result);
            Assert.Equal("test", challengeResult.Properties.RedirectUri);
        }
        
        [Fact]
        public void ExternalSignupCallbackAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ExternalSignUpCallback("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalSignupCallbackNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(nullExternalLoginInfo: false);

            var result = controller.ExternalSignUpCallback("test").Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SignUpViewModel>(viewResult.Model);
            Assert.NotNull(controller.ViewData["email"]);
            Assert.Equal("Signup", viewResult.ViewName);
        }
        
        [Fact]
        public void ExternalSignupCallbackNotAuthenticatedPostNullExternalLoginInfoTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(nullExternalLoginInfo: true);

            var result = controller.ExternalSignUpCallback("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/signup?returnUrl=test", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalLoginCallbackAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ExternalLoginCallback("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalLoginCallbackNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(nullExternalLoginInfo: false, externalLoginSuccess: true);

            var result = controller.ExternalLoginCallback("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("test", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalLoginCallbackNotAuthenticatedCannotLoginPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(nullExternalLoginInfo: false);

            var result = controller.ExternalLoginCallback("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/signup", redirectResult.Url);
        }
        
        [Fact]
        public void ExternalLoginCallbackNotAuthenticatedPostNullExternalLoginInfoTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(nullExternalLoginInfo: true);

            var result = controller.ExternalLoginCallback("test").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/login?returnUrl=test", redirectResult.Url);
        }
        
        [Fact]
        public void ForgotPasswordNotAuthenticatedGetTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.ForgotPassword();
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ForgotPasswordViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void ForgotPasswordAuthenticatedGetTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ForgotPassword();
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }

        [Fact]
        public void ForgotPasswordAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ForgotPassword(new ForgotPasswordViewModel()).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ForgotPasswordNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(findsByEmail: true);

            var result = controller.ForgotPassword(new ForgotPasswordViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("ForgotPasswordEmailSent", viewResult.ViewName);
        }
        
        [Fact]
        public void ForgotPasswordNotAuthenticatedPostInvalidEmailTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(findsByEmail: false);

            var result = controller.ForgotPassword(new ForgotPasswordViewModel()).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ForgotPasswordViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public void ConfirmEmailAuthenticatedGetTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ConfirmEmail("id", "token").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ConfirmEmailNotAuthenticatedGetTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(confirmableEmail: true);

            var result = controller.ConfirmEmail(Guid.NewGuid().ToString(), "token").Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("EmailConfirmed", viewResult.ViewName);
        }
        
        [Fact]
        public void ConfirmEmailAuthenticatedGetInvalidLinkTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(confirmableEmail: false);

            var result = controller.ConfirmEmail(null, "token").Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
            
            result = controller.ConfirmEmail(Guid.NewGuid().ToString(), "token").Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("InvalidEmailConfirmationLink", viewResult.ViewName);
        }

        [Fact]
        public void ResetPasswordAuthenticatedGetTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ResetPassword(Guid.NewGuid().ToString(), "token");
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }
        
        [Fact]
        public void ResetPasswordNotAuthenticatedGetTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.ResetPassword(Guid.NewGuid().ToString(), "token");
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ResetPasswordViewModel>(viewResult.Model);
        }
        
        [Fact]
        public void ResetPasswordNotAuthenticatedGetInvalidLinkTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser();

            var result = controller.ResetPassword(Guid.NewGuid().ToString(), null);
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }

        [Fact]
        public void ResetPasswordAuthenticatedPostTest()
        {
            var controller = GetControllerWithAuthenticatedUser();

            var result = controller.ResetPassword(new ResetPasswordViewModel()).Result;
            var redirectResult = Assert.IsType<RedirectResult>(result);
            Assert.Equal("/", redirectResult.Url);
        }

        [Fact]
        public void ResetPasswordNotAuthenticatedPostTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(resetablePassword: true);

            var result = controller.ResetPassword(new ResetPasswordViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password",
                UserId = Guid.NewGuid().ToString(),
                Token = "token"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("PasswordChanged", viewResult.ViewName);
        }
        
        [Fact]
        public void ResetPasswordNotAuthenticatedPostPasswordDoNotMatchTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(resetablePassword: true);

            var result = controller.ResetPassword(new ResetPasswordViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password_",
                UserId = Guid.NewGuid().ToString(),
                Token = "token"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ResetPasswordViewModel>(viewResult.Model);
            Assert.True(controller.ModelState.ErrorCount > 0);
        }

        [Fact]
        public void ResetPasswordNotAuthenticatedPostInvalidLinkTest()
        {
            var controller = GetControllerWithNotAuthenticatedUser(resetablePassword: false);

            var result = controller.ResetPassword(new ResetPasswordViewModel
            {
                NewPassword = "password",
                ConfirmNewPassword = "password",
                UserId = Guid.NewGuid().ToString(),
                Token = "token"
            }).Result;
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("InvalidPasswordResetLink", viewResult.ViewName);
        }
    }
}