using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.response;


namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.api
{
    public interface PolicyService
    {
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="username">userName</param>
        /// <param name="password">Password</param>
        /// <returns>LoginResp</returns>
        LoginResp Login(string username, string password);

        /// <summary>
        /// Calculate
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="param"></param>
        /// <returns>CalculationResp</returns>
        CalculationResp Calculate(string token, CalculationParams param);


        /// <summary>
        /// Issue
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="policy"></param>
        /// <returns>IssuedResp</returns>
        IssuedResp Issue(string token, Policy policy);

         
        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="policyId">issue success response policyNo</param>
        /// <param name="filePath">Specify the download path</param>
        void Download(string token, string policyNo, string filePath);

    }
}
