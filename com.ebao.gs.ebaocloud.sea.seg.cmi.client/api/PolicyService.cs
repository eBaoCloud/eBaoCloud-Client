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
		/// 登陆，必须登陆成功取到token后才能进行其它API调用
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <returns></returns>
		LoginResp Login(string username, string password);

		/// <summary>
		/// calculate
		/// </summary>
		/// <param name="token"></param>
		/// <param name="param"></param>
		/// <returns></returns>
		CalculationResp Calculate(string token, CalculationParams param);


		/// <summary>
		/// issue
		/// </summary>
		/// <param name="token"></param>
		/// <param name="policy"></param>
		/// <returns></returns>
		IssuedResp Issue(string token, Policy policy);

		void Download(string token,string policyId, string filePath);
	}
}
