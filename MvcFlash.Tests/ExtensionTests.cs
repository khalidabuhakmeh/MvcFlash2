using System;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using MvcFlash.Core.Messages;
using MvcFlash.Core.Providers;
using MvcFlash.Tests.Fakes;
using Xunit;

namespace MvcFlash.Tests
{
    public class ExtensionTests
    {
        protected HtmlHelper<SimpleMessage> Helper { get; set; }

        public ExtensionTests()
        {
            Flash.Reset();
            Flash.Initialize(new FlashSettings {
                Messenger = new InMemoryFlashMessenger()
            });
            Helper = GetHtmlHelper(new SimpleMessage(), true);
        }

        [Fact]
        public void Can_call_flash_using_extension_method()
        {
            var result = Helper.Flash();
            result.Should().NotBeNull();
        }

        [Fact]
        public void Can_pop_messages_using_extension_methods()
        {
            Flash.Instance.Push(new SimpleMessage() {Title = "Hello"});
            var result = Helper.Flash();

            result.Should().NotBeNull();
            result.ToString().Should().Contain("Hello");
        }

        [Fact]
        public void Can_cast_an_object_to_type_of_T()
        {
            object example = new TestController();
            var result = example.Cast<TestController>();
            result.ShouldBeEquivalentTo(example);
        }

        [Fact]
        public void Cast_of_null_returns_default_of_T()
        {
            ((object) null).Cast<TestController>().Should().BeNull();
        }

        [Fact]
        public void Can_perform_a_named_format()
        {
            "{hello}, {world}!"
                .NamedFormat(new {hello = "Hello", world = "World"})
                .Should().Be("Hello, World!");
        }

        [Fact]
        public void Can_perform_a_params_format()
        {
            "{0}, {1}!"
                .StringFormat("hello", "world")
                .Should().Be("hello, world!");

            "{0}".StringFormat("hello")
                .Should().Be("hello");
        }

        [Fact]
        public void Empty_with_format_throws_exceptions()
        {
            Assert.Throws<ArgumentNullException>(() => ((string)null).NamedFormat(new { text = "text" }));
            Assert.Throws<ArgumentNullException>(() => StringExtensions.NamedFormat(((string)null), "text"));
        }

        public static HtmlHelper<TModel> GetHtmlHelper<TModel>(TModel model, bool clientValidationEnabled)
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new FakeViewEngine());

            var httpContext = new FakeHttpContext();

            var viewData = new FakeViewDataContainer { ViewData = new ViewDataDictionary<TModel>(model) };

            var routeData = new RouteData();
            routeData.Values["controller"] = "home";
            routeData.Values["action"] = "index";

            ControllerContext controllerContext = new FakeControllerContext(new TestController());

            var viewContext = new FakeViewContext(controllerContext, "MyView", routeData);
            viewContext.HttpContext = httpContext;
            viewContext.ClientValidationEnabled = clientValidationEnabled;
            viewContext.UnobtrusiveJavaScriptEnabled = clientValidationEnabled;
            viewContext.FormContext = new FakeFormContext();

            var htmlHelper = new HtmlHelper<TModel>(viewContext, viewData);
            return htmlHelper;
        }

        public class TestController : Controller
        {}
    }

  
}
