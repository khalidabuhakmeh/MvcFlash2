MvcFlash 2 : Flash Messaging for ASP.Net MVC 4
===

There are times where you would like to pass a message up to the view, but you aren’t sure where the redirects will end up last. That is where MvcFlash comes in. You push messages into MvcFlash and then call Flash() when you need the messages to appear. A super simple implementation that just works. Download the source and run the sample to see some of the things you can do.

** Messages are persistent and will exist as long as they are not flashed, so you will not have to worry about redirects **

Installation
===

    Install-Package MvcFlash.Web

or

	Install-Package MvcFlash.Core

MvcFlash.Web includes starter templates for your solution. See below.

Implementation
===

In version 2 of MvcFlash, I have adopted multiple ways for you to implment MvcFlash into your application: Out of the Box or using IoC (DependencyResolver).

MvcFlash will attempt to resolve the interface of type [IFlashMessenger] (https://github.com/khalidabuhakmeh/MvcFlash2/blob/master/MvcFlash.Core/IFlashMessenger.cs) from the DependencyResolver in the MVC framework. If it is not resolved, then it will end up using the default implmentation, which is the [HttpSessionFlashMessenger] (https://github.com/khalidabuhakmeh/MvcFlash2/blob/master/MvcFlash.Core/Providers/HttpSessionFlashMessenger.cs). As you may have guessed, the HttpSessionFlashMessenger uses the SessionProvider in your application.

In The Controller
---

Out of the Box:

    Flash.Instance.Success("Well Done!", "You successfully read this important alert message.");
    Flash.Instance.Warning("Warning!", "You should really read this message");
    Flash.Instance.Info("Info!", "you can read this... if you want.");
    Flash.Instance.Error("Error!", "The sky is falling!");


This way works, but I recommend creating a base controller or by injecting the IFlashMessenger as a dependency.

Base Controller :

    public abstract class ApplicationController : Controller
    {
        protected virtual IFlashPusher Flash { get; private set; }

        protected ApplicationController()
        {
            Flash = MvcFlash.Core.Flash.Instance;
        }
    }

In Your Views
---

All you need to have in your views to display your messages is the following:

    @using MvcFlash.Core.Extensions;
    @Html.Flash()

Your messages will be displayed in Last in First Out fashion (LIFO).


Display
---

Html.Flash() uses MVC's DisplayFor attribute to give you the most flexibility when templating your messages. That means you can easily create all different types of messages and corresponding views with relative ease. The default views provided for you are below and are styled to work with [Twiiter Bootstrap](http://twitter.github.com/bootstrap/) out of the box.

SimpleMessages (the default for MvcFlash Extension Methods) :

    @model MvcFlash.Core.Messages.SimpleMessage
	<div class="alert alert-@Model.MessageType">
	  <button type="button" class="close" data-dismiss="alert">×</button>
	  <strong>@Model.Title!</strong> @Model.Content
	  @if (Model.Data != null) {
	    @Html.DisplayFor(m => m.Data)
	  }
	</div>

MessageBase (the catch all)

    @model MvcFlash.Core.Messages.MessageBase
	<div class="alert alert-@Model.MessageType">
	  <button type="button" class="close" data-dismiss="alert">×</button>
	  <strong>@Model.Title!</strong> @Model.Content
	</div>


Since Html.Flash() uses DisplayFor, you can create your own Messages (inheriting from MessageBase), and still leverage a custom template for your unique message. Examples of this can be found in the sample solution.

Notes
===

If you feel there should be any added features, please feel free to fork and make your additions. Also, please be sure to unit test your changes.

Thanks,

Khalid Abuhakmeh
