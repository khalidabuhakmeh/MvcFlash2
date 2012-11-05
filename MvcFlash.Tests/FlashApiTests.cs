using FluentAssertions;
using MvcFlash.Core;
using MvcFlash.Core.Extensions;
using MvcFlash.Core.Messages;
using MvcFlash.Core.Providers;
using Xunit;

namespace MvcFlash.Tests
{
    public class FlashApiTests
    {
        private IFlashMessenger Flash { get; set; }

        public FlashApiTests()
        {
            Flash = new InMemoryFlashMessenger();
            Core.Flash.Reset();
        }

        [Fact]
        public void Can_push_a_new_message_on_to_the_flash_messages_stack()
        {
            Flash.Push(new SimpleMessage<object>
            {
                Title = "This is a test",
                MessageType = "test",
                Content = "this is the message I want to tell you"
            });

            Flash.Count.Should().Be(1);

            var message = Flash.Pop();

            Flash.Count.Should().Be(0);

            message.Should().NotBeNull();
            message.MessageType.Should().Be("test");
        }

        [Fact]
        public void Can_push_a_simple_message_with_data_onto_the_flash_message_stack()
        {

            Flash.Push(new SimpleMessage<Test>
            {
                Data = new Test
                {
                    ErrorCode = 500,
                    Link = "http://msdn.com"
                }
            });

            Flash.Count.Should().Be(1);

            var message = Flash.Pop();

            message.Should().BeAssignableTo<MessageBase>();
            message.As<SimpleMessage<Test>>().Data.Should().NotBeNull();
            message.As<SimpleMessage<Test>>().Data.ErrorCode.Should().Be(500);
            message.As<SimpleMessage<Test>>().Data.Link.Should().Be("http://msdn.com");
        }

        [Fact]
        public void Can_push_100_messages_onto_the_flash_message_stack()
        {
            for (int i = 0; i < 100; i++)
                Flash.Push(new SimpleMessage<object>());

            Flash.Count.Should().Be(100);
        }

        [Fact]
        public void Can_push_unique_message_onto_flash_message_stack_with_result_of_one_message()
        {
            for (int i = 0; i < 100; i++)
                Flash.Push(new SimpleMessage<object> { Id = "test" });

            Flash.Count.Should().Be(1);
        }

        [Fact]
        public void Can_push_a_success_message()
        {
            var message = Flash.Success("success!", "this is so successful!");
            
            message.Should().NotBeNull();
            message.MessageType.Should().Be(Core.Flash.Types.Success);

            Flash.Count.Should().Be(1);
        }

        [Fact]
        public void Can_push_a_error_message()
        {
            var message = Flash.Error("error!", "this is so not successful!");

            message.Should().NotBeNull();
            message.MessageType.Should().Be(Core.Flash.Types.Error);

            Flash.Count.Should().Be(1);
        }

        [Fact]
        public void Can_push_a_warning_message()
        {
            var message = Flash.Warning("error!", "this is so not successful!");

            message.Should().NotBeNull();
            message.MessageType.Should().Be(Core.Flash.Types.Warning);

            Flash.Count.Should().Be(1);
        }

        [Fact]
        public void Can_push_an_info_message()
        {
            var message = Flash.Info("Info!", "carry on.");

            message.Should().NotBeNull();
            message.MessageType.Should().Be(Core.Flash.Types.Info);

            Flash.Count.Should().Be(1);
        }

        [Fact]
        public void Can_change_type_of_success_using_configuration()
        {
            Core.Flash.Types.Success = "a-ok";
            var message = Flash.Success("hello");
            message.MessageType.Should().Be("a-ok");
        }

        [Fact]
        public void Can_push_a_success_message_with_data()
        {
            var message = Flash.Success(data: new Test());
            message.As<SimpleMessage<Test>>().Data.Should().NotBeNull();
        }

        [Fact]
        public void Can_push_a_unique_success_message()
        {
            for (int i = 0; i < 100; i++)
                Flash.Success(id: "unique");

            Flash.Count.Should().Be(1);
        }

        public class Test
        {
            public int ErrorCode { get; set; }
            public string Link { get; set; }
        }

    }
}
