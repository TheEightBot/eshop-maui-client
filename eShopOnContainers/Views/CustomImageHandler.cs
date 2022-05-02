using System;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

#if IOS
using System.Reflection;
using UIKit;
#endif

namespace eShopOnContainers.Views
{
	public partial class CustomImageHandler : ImageHandler
	{
		public CustomImageHandler()
		{
		}

#if IOS

        protected override void ConnectHandler(UIImageView platformView)
        {
            //base.ConnectHandler(platformView);

            if (platformView is MauiImageView imageView)
                imageView.WindowChanged += OnWindowChanged;
        }

        protected override void DisconnectHandler(UIImageView platformView)
        {
            //base.DisconnectHandler(platformView);

       //     try
       //     {
			    //var pv = PlatformView;
       //     }
       //     catch (InvalidOperationException ioe)
       //     {
       //         Console.WriteLine(ioe);
       //     }

			if (platformView is MauiImageView imageView)
            {
                imageView.WindowChanged -= OnWindowChanged;
            }

			SourceLoader.Reset();
		}

        void OnWindowChanged(object? sender, EventArgs e)
        {
            UpdateValue(nameof(Microsoft.Maui.IImage.Source));
        }
#endif
    }
}