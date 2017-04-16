using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class UploadFileExtraData
    {
        public string policyId;
        public string docName;
        public string docType;

    }

    public class UploadFileParams
    {
        public UploadFileExtraData uploadExtraData;
        public FileStream fileStream;
    }
}
