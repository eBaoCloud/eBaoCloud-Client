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
        /// 登陆，必须登陆成功取到token后才能进行其它API调用
        /// </summary>
        /// <param name="username">userName</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        LoginResp Login(string username, string password);

        /// <summary>
        /// calculate
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="param"></param>
        /// <returns></returns>
        CalculationResp Calculate(string token, CalculationParams param);


        /// <summary>
        /// issue
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="policy"></param>
        /// <returns></returns>
        IssuedResp Issue(string token, Policy policy);

         
        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="token">login success response token</param>
        /// <param name="policyId"></param>
        /// <param name="filePath"></param>
        void Download(string token, string policyId, string filePath);

    }
}
