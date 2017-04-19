using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace com.ebaocloud.client.thai.seg.cmi.parameters
{
	public class UploadFileParams
	{
		public JObject uploadExtraData;
		public FileInfo fileInfo;
	}
}
