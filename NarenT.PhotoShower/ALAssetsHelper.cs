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
	public class ALAssetsHelper
	{
		public IEnumerable<ALAssetsGroup> GetGroups()
		{
			ConcurrentBag<ALAssetsGroup> groupUrls = new ConcurrentBag<ALAssetsGroup>();
			var waitHandle = new AutoResetEvent(false);
			Foundation.NSError error = null;
			var assetsLibrary = new ALAssetsLibrary ();
			assetsLibrary.Enumerate(ALAssetsGroupType.All, 
				(ALAssetsGroup group, ref bool stop) => {
					if (group != null) {
						stop = false;
						groupUrls.Add (group);
					} else {
						waitHandle.Set ();
					}
				}, 
				(e) => {
					error = e; 
				});

			if (error != null) {
				throw new Exception(error.Description);
			}

			waitHandle.WaitOne ();
			return groupUrls;
		}

		public IEnumerable<ALAsset> GetGroupAssets(ALAssetsGroup assetsGroup)
		{
			var waitHandle = new AutoResetEvent (false);
			waitHandle.Reset ();
			var assets = new ConcurrentBag<ALAsset> ();
			assetsGroup.Enumerate ((ALAsset asset, nint index, ref bool stop) => {
				if(asset != null)
				{
					assets.Add(asset);
					stop = false;
				}
				else 
				{
					waitHandle.Set();
				}
			});

			waitHandle.WaitOne ();
			return assets;
		}

		public ALAsset GetAsset(string url)
		{
			var waitHandle = new AutoResetEvent (false);
			waitHandle.Reset ();
			var al = new ALAssetsLibrary ();
			ALAsset ala = null;
			al.AssetForUrl (NSUrl.FromString (url),
				(a) => {
					ala = a;
					waitHandle.Set ();
				},
				(e) => {
					waitHandle.Set ();
				});

			waitHandle.WaitOne ();
			return ala;
		}
	}
	
}