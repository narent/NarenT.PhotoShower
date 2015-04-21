using System;
using NarenT.HTTP;
using NarenT.HTTP.Actions;
using AssetsLibrary;
using System.Collections.Generic;
using System.Threading;
using System.Collections.Concurrent;
using Foundation;
using System.Linq;
using NarenT.PhotoShower.Models;

namespace NarenT.PhotoShower
{

	public class PhotosGroupsAction : HttpActionBase
	{
		public PhotosGroupsAction ()
		{
		}

		public override ActionResult GET (System.Net.HttpListenerContext context)
		{
			var aLAssetsHelper = new ALAssetsHelper ();
			var groups = aLAssetsHelper.GetGroups ();
			var photoGroups = groups.Select (g => new PhotoGroup () 
				{ 
					Name = g.Name, 
					Description = g.Description, 
					Url = g.PropertyUrl.AbsoluteString,
					Photos = aLAssetsHelper.GetGroupAssets(g)
						.Select(ga => new Models.PhotoDescription() { Url = ga.AssetUrl.AbsoluteString }).ToList()
				});

			var pgv = new PhotoGroupsView () { Model = photoGroups.ToList() };
			return new StringActionResult(pgv.GenerateString());
		}

		public override ActionResult POST (System.Net.HttpListenerContext context)
		{
			return new ActionResult () {
				HttpStatusCode = System.Net.HttpStatusCode.BadRequest
			};
		}
	}
}