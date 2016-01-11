using System;
using System.Web;
using FluentAssertions;
using MvcFlash.Core.Messages;
using MvcFlash.Core.Providers;
using Xunit;

namespace MvcFlash.Tests.Providers
{
    public class CookieStorageFlashMessengerTests
    {
        private CookieStorageFlashMessenger _sut;
        private FakeCookieProvider _cookieProvider;
        private string _cookieName = "__FLASH__";

        public CookieStorageFlashMessengerTests()
        {
            CreateSystemUnderTest();
        }

        [Fact]
        public void Initial_count_is_zero()
        {
            _sut.Count.ShouldBeEquivalentTo(0);
        }

        [Fact]
        public void Initially_there_is_no_cookie_in_the_response()
        {
            ResponseCookieValue().Should().BeNull();
        }

        [Fact]
        public void After_adding_message_cookie_is_present_in_reponse()
        {
            _sut.Push(new SimpleMessage {MessageType = "info", Title = "Hello"});

            ResponseCookieValue().Should().Contain("\"MessageType\":\"info\"");
            ResponseCookieValue().Should().Contain("\"Title\":\"Hello\"");
        }

        [Fact]
        public void Adding_null_message_throws_exception()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.Push(null));

            exception.Message.ShouldBeEquivalentTo("message cannot be null\r\nParameter name: message");
        }

        [Fact]
        public void Adding_message_of_wrong_type_throws_exception()
        {
            var exception = Assert.Throws<ArgumentException>(
                () => _sut.Push(new OtherTypeOfMessage()));

            exception.Message.ShouldBeEquivalentTo(
                "Cookie Flash Storage only allows instances of SimpleMessage to be used!\r\nParameter name: message");
        }

        [Fact]
        public void After_adding_two_messages_both_present_in_response_cookie()
        {
            _sut.Push(new SimpleMessage { Title = "hello" });
            _sut.Push(new SimpleMessage { Title = "world" });

            Console.WriteLine(ResponseCookieValue());

            ResponseCookieValue().Should().Contain("\"Title\":\"hello\"");
            ResponseCookieValue().Should().Contain("\"Title\":\"world\"");
        }

        [Fact]
        public void After_adding_messages_count_is_correct()
        {
            _sut.Count.ShouldBeEquivalentTo(0);
            _sut.Push(new SimpleMessage { Title = "hello" });
            _sut.Count.ShouldBeEquivalentTo(1);
            _sut.Push(new SimpleMessage { Title = "world" });
            _sut.Count.ShouldBeEquivalentTo(2);
        }

        [Fact]
        public void Message_already_present_count_is_correct()
        {
            SetRequestCookieValue(@"{'m1':{'Id':'m1','Title':'hello'}}");

            _sut.Count.ShouldBeEquivalentTo(1);
        }

        [Fact]
        public void Two_messages_already_present_count_is_correct()
        {
            SetRequestCookieValue(
                @"{'m1':{'Id':'m1','Title':'hello'}, 'm2':{'Id':'m2','Title':'world'}}");

            _sut.Count.ShouldBeEquivalentTo(2);
        }

        [Fact]
        public void Pop_returns_null_if_no_messages_available()
        {
            _sut.Pop().Should().BeNull();
        }

        [Fact]
        public void Pop_returns_message_already_present()
        {
            SetRequestCookieValue(@"{'m1':{'Id':'m1','Title':'hello'}}");

            var message = _sut.Pop();

            message.Id.ShouldBeEquivalentTo("m1");
            message.Title.ShouldBeEquivalentTo("hello");
        }

        [Fact]
        public void Pop_returns_message_just_added()
        {
            _sut.Push(new SimpleMessage {Id = "m1", Title = "hello"});

            var message = _sut.Pop();

            message.Id.ShouldBeEquivalentTo("m1");
            message.Title.ShouldBeEquivalentTo("hello");
        }

        [Fact]
        public void After_pop_count_is_decremented()
        {
            _sut.Push(new SimpleMessage {Id = "m1", Title = "hello"});
            _sut.Push(new SimpleMessage {Id = "m2", Title = "world"});

            _sut.Count.ShouldBeEquivalentTo(2);

            _sut.Pop();
            _sut.Count.ShouldBeEquivalentTo(1);
            _sut.Pop();
            _sut.Count.ShouldBeEquivalentTo(0);
            _sut.Pop();
            _sut.Count.ShouldBeEquivalentTo(0);
        }

        [Fact]
        public void No_message_recently_added_Response_cookie_not_set()
        {
            SetRequestCookieValue(@"{'m1':{'Id':'m1','Title':'hello'}}");

            ResponseCookieValue().Should().BeNull();
        }

        [Fact]
        public void Message_available_and_new_one_added_added_Response_cookie_contains_both()
        {
            SetRequestCookieValue(@"{'m1':{'Id':'m1','Title':'hello'}}");
            _sut.Push(new SimpleMessage { Id = "m2", Title = "world" });

            ResponseCookieValue().Should().Contain("\"Title\":\"hello\"");
            ResponseCookieValue().Should().Contain("\"Title\":\"world\"");
        }

        [Fact]
        public void Two_messages_available_Popping_one_sets_response_cookie()
        {
            SetRequestCookieValue(
                @"{'m1':{'Id':'m1','Title':'hello'}, 'm2':{'Id':'m2','Title':'world'}}");

            _sut.Pop();

            ResponseCookieValue().Should().Contain("\"Title\":\"hello\"");
        }

        [Fact]
        public void Calling_Clear_discards_messages_and_saves_expired_response_cookie()
        {
            SetRequestCookieValue(
                @"{'m1':{'Id':'m1','Title':'hello'}, 'm2':{'Id':'m2','Title':'world'}}");

            _sut.Clear();

            var yesterday = DateTime.Now.AddDays(-1);

            ResponseCookie().Expires.Should().BeBefore(yesterday);
            ResponseCookieValue().Should().BeNullOrEmpty();
        }

        [Fact]
        public void Calling_Clear_resets_count()
        {
            SetRequestCookieValue(
                @"{'m1':{'Id':'m1','Title':'hello'}, 'm2':{'Id':'m2','Title':'world'}}");

            _sut.Clear();

            _sut.Count.ShouldBeEquivalentTo(0);
        }

        

        private void CreateSystemUnderTest()
        {
            _cookieProvider = new FakeCookieProvider();
            _sut = new CookieStorageFlashMessenger(_cookieProvider, _cookieName);
        }
        private HttpCookie ResponseCookie()
        {
            return _cookieProvider.ResponseCookies[_cookieName];
        }
        private string ResponseCookieValue()
        {
            return ResponseCookie() != null ? ResponseCookie().Value : null;
        }
        private void SetRequestCookieValue(string value)
        {
            _cookieProvider.RequestCookies.Set(new HttpCookie(_cookieName, value));
        }

        internal class FakeCookieProvider : ICookieProvider
        {
            public FakeCookieProvider()
            {
                RequestCookies = new HttpCookieCollection();
                ResponseCookies = new HttpCookieCollection();
            }

            public HttpCookieCollection RequestCookies { get; private set; }
            public HttpCookieCollection ResponseCookies { get; private set; }

            public HttpCookie GetRequestCookie(string cookieName)
            {
                return RequestCookies[cookieName];
            }

            public void SetResponseCookie(HttpCookie cookie)
            {
                ResponseCookies.Set(cookie);
            }
        }

        public class OtherTypeOfMessage : MessageBase { }
    }
}