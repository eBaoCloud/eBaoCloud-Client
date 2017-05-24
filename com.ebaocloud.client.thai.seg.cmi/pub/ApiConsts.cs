using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace com.ebaocloud.client.thai.seg.cmi.pub
{
    class ApiConsts
    {
        public const string API_LOGIN = "/gi/api/users/login";
		public const string API_STRUCTURE = "/gi/api/b2b/product/structure";
        public const string API_CALCULATE = "/gi/api/std/b2b/policies/calcexternal";
        public const string API_BUY = "/gi/api/std/b2b/policies/buy";
        public const string API_BIND = "/gi/api/std/b2b/policies/bind";
        public const string API_CONFRIM = "/gi/api/std/b2b/policies/confirm/";
        public const string API_PAY = "/gi/api/std/b2b/policies/pay/";
        public const string API_PAYMENT_STATUS = "/gi/api/std/b2b/policies/paymentstatus/";
        public const string API_DOCS = "/gi/api/std/b2b/policies/docs";
        public const string API_COVERAGES = "/gi/api/std/b2b/policies/coverageexternal";
        public const string API_VEHICLE = "/gi/api/thai/smart/vehicle";

		public const string API_GET_PRINTED_FILES = "/gi/api/std/b2b/policies/printexternal";
		public const string API_DOWNLOAD_PRINTED_FILE = "/gi/api/std/b2b/policies/print/download";
        public const string API_ADDRESS_MATCHING = "/gi/api/misc/address/smartmatching";

        public const string API_MISC_VEHICLE_MAKE = "/gi/api/misc/thai/SEG_TH/motor/makes";
        public const string API_MISC_VEHICLE_MAKE_MODEL = "/gi/api/misc/thai/SEG_TH/motor/models/";
        public const string API_MISC_VEHICLE_MODEL_YEAR = "/gi/api/misc/thai/SEG_TH/motor/modelyears/";
        public const string API_MISC_VEHICLE_MODEL_INFO = "/gi/api/misc/thai/SEG_TH/motor/submodels/";

        public const string API_MISC_ADDRESS_PROVINCE = "/gi/api/misc/address/provinces";
        public const string API_MISC_ADDRESS_DISTRICT = "/gi/api/misc/address/districts/";
        public const string API_MISC_ADDRESS_SUB_DISTRICT = "/gi/api/misc/address/subdistricts/";


        public const string API_MISC_MASTER_DATA = "/gi/api/misc/mastertab";

    }
}
