using System;
using System.Collections.Generic;

namespace NarenT.PhotoShower.Models
{
	public class PhotoGroup
	{
		public string Name {
			get;
			set;
		}

		public string Description {
			get;
			set;
		}

		public string Url {
			get;
			set;
		}

		public List<PhotoDescription> Photos {
			get;
			set;
		}
	}
}

