using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using com.ebaocloud.client.thai.seg.vmi.api;
using com.ebaocloud.client.thai.seg.vmi.pub;
using com.ebaocloud.client.thai.seg.vmi.parameters;
using com.ebaocloud.client.thai.seg.vmi.response;
using System.IO;

namespace com.ebaocloud.client.thai.seg.vmi.factory
{
    public class ServiceFactory
    {
        internal static String token { set; get; }
        internal static EnvironmentService environmentService;
        internal static MasterDataService masterDataService;
        internal static PolicyService policyService;

        public static void Login(EnvironmentType envType, String userName, String password)
        {
            if (environmentService == null)
            {
                environmentService = new EnvironmentServiceImpl();
                environmentService.SetEnvironmentType(envType);
            }
            if (policyService == null)
            {
                policyService = new PolicyServiceImpl();
            }
            LoginResp loginResp = policyService.Login(userName, password);
            token = loginResp.token;
        }

        public static CalculationResp Calculate(CalculationParams param)
        {
            ParamValidate();
            CalculationResp resp = policyService.Calculate(token, param);
            return resp;
        }

        public static IssuedResp Issue(Policy policy)
        {
            ParamValidate();
            IssuedResp resp = policyService.Issue(token, policy);
            return resp;
        }

        public static void DownloadPolicyForms(string policyNo, DirectoryInfo destinationFolder)
        {
            ParamValidate();
            policyService.DownloadPolicyForms(token, policyNo, destinationFolder);
        }

        public static MasterDataService GetMasterDataService()
        {
            if (masterDataService == null)
            {
                masterDataService = new MasterDataServiceImpl();
            }
            return masterDataService;
        }

        private static void ParamValidate()
        {
            if (token == null || token == "")
            {
                throw new Exception("Not logged in yet or loginToken is expired");
            }
            if (policyService == null)
            {
                policyService = new PolicyServiceImpl();
            }
        }

    }
}
