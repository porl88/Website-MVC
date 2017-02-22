namespace MVC.WebUI.Tests.Account
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Account;
    using Moq;
    using Services.Account;
    using Services.Account.Transfer;
    using Services.Message;
    using Services.Message.Transfer;

    // http://www.asp.net/mvc/overview/older-versions-1/unit-testing/creating-unit-tests-for-asp-net-mvc-applications-cs

    [TestClass]
    public class AccountControllerTests
    {
        private Mock<HttpContextBase> mockContext;

        [TestInitialize]
        public void Init()
        {
            this.mockContext = new Mock<HttpContextBase>();
        }

        [TestMethod]
        public void Create()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            var controller = new AccountController(null, mockAuthenticationService.Object, null);

            // act
            var result = controller.Create() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void Create_Redirect()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService.SetupGet(m => m.IsAuthenticated).Returns(true);
            var controller = new AccountController(null, mockAuthenticationService.Object, null);

            // act
            var result = controller.Create() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        public void Create_Post_OK()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            var mockAccountService = new Mock<IAccountService>();
            mockAccountService
                .Setup(m => m.CreateAccount(It.IsAny<CreateAccountRequest>()))
                .Returns(new CreateAccountResponse
                {
                    Success = true,
                    ActivateAccountToken = "YYYYYYYYYYYY"
                });

            var mockMessagingService = new Mock<IMessageService>();
            var controller = new AccountController(mockAccountService.Object, mockAuthenticationService.Object, mockMessagingService.Object);


            //controller.Url = new UrlHelper(
            //    new RequestContext(this.mockContext.Object, new RouteData()),
            //    new RouteCollection()
            //);


            // mock url needed to mock the call to Url.Action
            //var mockUrl = new Mock<UrlHelper>();
            //mockUrl.Setup(x => x.Action("ActivateAccount", "Email", new { name = "FirstName", token = "token" })).Returns("/email/activateaccount?name=XXX&token=YYY");
            //controller.Url = mockUrl.Object;


            var model = new CreateAccountViewModel
            {
                Email = "email@email.com",
                Password = "XXXXXXXX",
                ConfirmPassword = "XXXXXXXX"
            };

            // act
            var result = controller.Create(model) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("LogIn", result.RouteValues["action"]);
            Assert.AreEqual(null, result.RouteValues["controller"]);
            Assert.IsNotNull(controller.TempData["SuccessMessage"]);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        public void Create_Post_Error()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();

            var mockAccountService = new Mock<IAccountService>();
            mockAccountService
                .Setup(m => m.CreateAccount(It.IsAny<CreateAccountRequest>()))
                .Returns(new CreateAccountResponse
                {
                    Success = false
                });

            var controller = new AccountController(mockAccountService.Object, mockAuthenticationService.Object, null);

            var model = new CreateAccountViewModel();

            // act
            var result = controller.Create(model) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.AreEqual(1, result.ViewData.ModelState.Count);
        }

        [TestMethod]
        public void Login()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            var returnUrl = "xxxx";
            var controller = new AccountController(null, mockAuthenticationService.Object, null);

            // act
            var result = controller.LogIn(returnUrl) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.AreEqual(returnUrl, result.ViewData["returnUrl"]);
        }

        [TestMethod]
        public void Login_Post_Success()
        {
            // arrange
            var userName = "XXX";
            var password = "YYY";
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.LogIn(It.Is<LoginRequest>(x => x.UserName == userName && x.Password == password)))
                .Returns(new LoginResponse
                {
                    Success = true,
                    IsAuthenticated = true
                });

            var returnUrl = "/home";
            var model = new LoginViewModel
            {
                UserName = userName,
                Password = password,
            };

            var controller = new AccountController(null, mockAuthenticationService.Object, null);
            var requestContext = new RequestContext(this.mockContext.Object, new RouteData());
            controller.Url = new UrlHelper(requestContext);

            // act
            var result = controller.LogIn(model, returnUrl) as RedirectResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(returnUrl, result.Url);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        public void Login_Post_Success_InvalidReturnUrl()
        {
            // arrange
            var userName = "XXX";
            var password = "YYY";
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.LogIn(It.Is<LoginRequest>(x => x.UserName == userName && x.Password == password)))
                .Returns(new LoginResponse
                {
                    Success = true,
                    IsAuthenticated = true
                });

            var returnUrl = "http://home";
            var model = new LoginViewModel
            {
                UserName = userName,
                Password = password,
            };

            var controller = new AccountController(null, mockAuthenticationService.Object, null);

            // mock the UrlHelper
            var requestContext = new RequestContext(this.mockContext.Object, new RouteData());
            controller.Url = new UrlHelper(requestContext);

            // act
            var result = controller.LogIn(model, returnUrl) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        public void Login_Post_Unauthenticated()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.LogIn(It.IsAny<LoginRequest>()))
                .Returns(new LoginResponse
                {
                    Success = false,
                    IsAuthenticated = false
                });

            var controller = new AccountController(null, mockAuthenticationService.Object, null);
            var requestContext = new RequestContext(this.mockContext.Object, new RouteData());
            controller.Url = new UrlHelper(requestContext);

            // act
            var result = controller.LogIn(new LoginViewModel(), string.Empty) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Login_Post_Error()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.LogIn(It.IsAny<LoginRequest>()))
                .Returns(new LoginResponse
                {
                    Success = false,
                    IsAuthenticated = false
                });

            var controller = new AccountController(null, mockAuthenticationService.Object, null);
            var requestContext = new RequestContext(this.mockContext.Object, new RouteData());
            controller.Url = new UrlHelper(requestContext);

            // act
            var result = controller.LogIn(new LoginViewModel(), string.Empty) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void LogOut()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            var controller = new AccountController(null, mockAuthenticationService.Object, null);

            // act
            var result = controller.LogOut() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.IsNotNull(controller.TempData["SuccessMessage"]);
            Assert.IsFalse(result.Permanent);
        }

        [TestMethod]
        public void RequestPassword()
        {
            // arrange
            var controller = new AccountController(null, null, null);

            // act
            var result = controller.RequestPassword() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [TestMethod]
        public void RequestPassword_Post_Success()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.ResetPasswordRequest(It.Is<RequestPasswordRequest>(x => x.UserName == "XXX")))
                .Returns(new RequestPasswordResponse
                {
                    Success = true,
                    Message = "XXX"
                });

            var mockMessageService = new Mock<IMessageService>();
            mockMessageService
                .Setup(m => m.SendMessage(It.IsAny<MessageRequest>()))
                .Returns(new MessageResponse
                {
                    Success = true
                });

            var model = new RequestPasswordViewModel
            {
                UserName = "XXX"
            };

            var controller = new AccountController(null, mockAuthenticationService.Object, mockMessageService.Object);

            // act
            var result = controller.RequestPassword(model) as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Login", result.RouteValues["action"]);
            Assert.IsNotNull(controller.TempData["SuccessMessage"]);
        }

        [TestMethod]
        public void RequestPassword_Post_Failure()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.ResetPasswordRequest(It.Is<RequestPasswordRequest>(x => x.UserName == "XXX")))
                .Returns(new RequestPasswordResponse
                {
                    Success = false,
                    Message = "XXX"
                });

            var model = new RequestPasswordViewModel
            {
                UserName = "XXX"
            };

            var controller = new AccountController(null, mockAuthenticationService.Object, null);

            // act
            var result = controller.RequestPassword(model) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void RequestPassword_Post_MessageFailure()
        {
            // arrange
            var mockAuthenticationService = new Mock<IAuthenticationService>();
            mockAuthenticationService
                .Setup(m => m.ResetPasswordRequest(It.Is<RequestPasswordRequest>(x => x.UserName == "XXX")))
                .Returns(new RequestPasswordResponse
                {
                    Success = true,
                    Message = "XXX"
                });

            var mockMessageService = new Mock<IMessageService>();
            mockMessageService
                .Setup(m => m.SendMessage(It.IsAny<MessageRequest>()))
                .Returns(new MessageResponse
                {
                    Success = false
                });

            var model = new RequestPasswordViewModel
            {
                UserName = "XXX"
            };

            // assert
            var controller = new AccountController(null, mockAuthenticationService.Object, mockMessageService.Object);

            // act
            var result = controller.RequestPassword(model) as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
        }
    }
}
