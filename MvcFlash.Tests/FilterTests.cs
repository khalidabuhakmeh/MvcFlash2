using System.Collections.Specialized;
using System.Web.Mvc;
using FluentAssertions;
using MvcFlash.Core;
using MvcFlash.Core.Filters;
using MvcFlash.Core.Messages;
using MvcFlash.Core.Providers;
using MvcFlash.Tests.Fakes;
using Xunit;

namespace MvcFlash.Tests
{
    public class FilterTests
    {
        public FilterTests()
        {
            Flash.Reset();
            Flash.Initialize(new FlashSettings {
                Messenger = new InMemoryFlashMessenger()
            });
        }

        [Fact]
        public void Can_clear_all_filter_messages_with_attribute()
        {
            var filter = new ClearFlashOnAjaxAttribute();
            var context = new FakeHttpContext(new NameValueCollection { { "X-Requested-With", "XMLHttpRequest" } });

            Flash.Instance.Push(new SimpleMessage());
            Flash.Instance.Count.Should().Be(1);

            filter.OnActionExecuted(new ActionExecutedContext { HttpContext = context });

            Flash.Instance.Count.Should().Be(0);
        }
    }
}
