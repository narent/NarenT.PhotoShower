using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using UIKit;
using NarenT.HTTP;
using System.Net.NetworkInformation;
using NarenT.Common;
using NarenT.HTTP.Actions;
using AssetsLibrary;
using CoreGraphics;

namespace NarenT.PhotoShower
{
	public class PhotoAction : HttpActionBase
	{
		public override ActionResult GET (System.Net.HttpListenerContext context)
		{
			var url = context.Request.QueryString.Get ("url");
			var size = context.Request.QueryString.Get ("size");

			var asset = new ALAssetsHelper ().GetAsset (url);
			if (asset == null) {
				return new ActionResult () { HttpStatusCode = System.Net.HttpStatusCode.NotFound };
			}

			CGImage img = null;
			switch (size) {
			case "f":
				img = asset.DefaultRepresentation.GetFullScreenImage ();
				break;
			default:
				img = asset.Thumbnail;
				break;
			}

			using (NSData imageData = new UIImage(img).AsPNG()) {
				Byte[] imageBytes = new Byte[imageData.Length];
				System.Runtime.InteropServices.Marshal.Copy(imageData.Bytes, imageBytes, 0, Convert.ToInt32(imageData.Length));
				return new ActionResult() { ContentType = MimeTypes.GetMimeType("png"), Data = imageBytes };
			}
		}

		public override ActionResult POST (System.Net.HttpListenerContext context)
		{
			return new ActionResult () {
				HttpStatusCode = System.Net.HttpStatusCode.BadRequest
			};
		}
	}
}

