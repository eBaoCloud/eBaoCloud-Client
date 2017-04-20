using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ebaocloud.client.thai.seg.cmi.parameters;
using com.ebaocloud.client.thai.seg.cmi.response;

namespace com.ebaocloud.client.thai.seg.cmi.api
{
	public interface PolicyService
	{
        /// <summary>
        /// If the login is successful you will recieve a token, normally you need to cache it to call the other APIs.
        /// </summary>
        /// <param name="username">name</param>
        /// <param name="password">password</param>
        /// <returns>LoginResp</returns>
        LoginResp Login(string username, string password);

        /// <summary>
        /// If the calculation is successful, the response object will show you some informations including totalFeeAmount,totalTaxAmount,payablePremium...
        /// </summary>
        /// <param name="token">login response token</param>
        /// <param name="param"></param>
        /// <returns>CalculationResp</returns>
        CalculationResp Calculate(string token, CalculationParams param);


        /// <summary>
        /// If success you will recieve a policy number.
        /// </summary>
        /// <param name="token">login response token</param>
        /// <param name="policy"></param>
        /// <returns>IssuedResp</returns>
        IssuedResp Issue(string token, Policy policy);


        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="policyNo">issue success response policyNo</param>
        /// <param name="filePath">Specify the download path</param>
		void Download(string token, string policyNo, string filePath);
	}
}
