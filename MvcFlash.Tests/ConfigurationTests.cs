using FluentAssertions;
using MvcFlash.Core;
using MvcFlash.Core.Providers;
using Xunit;

namespace MvcFlash.Tests
{
    public class ConfigurationTests
    {
        public ConfigurationTests()
        {
            Flash.Reset();
        }

        [Fact]
        public void Can_use_settings_to_set_flash_messenger()
        {
            Flash.Initialize(new FlashSettings
            {
                Messenger = new InMemoryFlashMessenger()
            });
            Flash.Instance.Should().NotBeNull();
            Flash.Instance.Should().BeAssignableTo<InMemoryFlashMessenger>();
        }

        [Fact]
        public void Success_should_be_defaulted_to_success()
        {
            Flash.Types.Success.Should().Be("success");
        }

        [Fact]
        public void Warning_should_be_defaulted_to_warning()
        {
            Flash.Types.Warning.Should().Be("warning");
        }

        [Fact]
        public void Info_should_be_defaulted_to_info()
        {
            Flash.Types.Info.Should().Be("info");
        }

        [Fact]
        public void Error_should_be_defaulted_to_error()
        {
            Flash.Types.Error.Should().Be("error");
        }
    }
}
