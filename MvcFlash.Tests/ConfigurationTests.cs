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
            Core.MvcFlash.Reset();
        }

        [Fact]
        public void Can_use_settings_to_set_flash_messenger()
        {
            Core.MvcFlash.Initialize(new FlashSettings
            {
                Messenger = new InMemoryFlashMessenger()
            });
            Core.MvcFlash.Instance.Should().NotBeNull();
            Core.MvcFlash.Instance.Should().BeAssignableTo<InMemoryFlashMessenger>();
        }

        [Fact]
        public void Success_should_be_defaulted_to_success()
        {
            Core.MvcFlash.Types.Success.Should().Be("success");
        }

        [Fact]
        public void Warning_should_be_defaulted_to_warning()
        {
            Core.MvcFlash.Types.Warning.Should().Be("warning");
        }

        [Fact]
        public void Info_should_be_defaulted_to_info()
        {
            Core.MvcFlash.Types.Info.Should().Be("info");
        }

        [Fact]
        public void Error_should_be_defaulted_to_error()
        {
            Core.MvcFlash.Types.Error.Should().Be("error");
        }
    }
}
