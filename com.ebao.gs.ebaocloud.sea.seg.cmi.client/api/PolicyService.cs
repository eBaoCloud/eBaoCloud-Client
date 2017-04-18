using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.response;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.api
{
	public interface PolicyService
	{
		/// <summary>
		/// Login
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns>LoginResp</returns>
		LoginResp Login(string username, string password);

        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="token"></param>
        /// <param name="param"></param>
        /// <returns>CalculationResp</returns>
        CalculationResp Calculate(string token, CalculationParams param);


        /// <summary>
        /// Issue
        /// </summary>
        /// <param name="token"></param>
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
