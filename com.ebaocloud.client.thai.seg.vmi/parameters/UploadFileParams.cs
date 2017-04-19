using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;

namespace com.ebaocloud.client.thai.seg.vmi.parameters
{

    public class UploadFileParams
    {
        public JObject uploadExtraData;
        public FileInfo fileInfo;
    }
}
