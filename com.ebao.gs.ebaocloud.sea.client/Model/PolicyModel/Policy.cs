using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ebao.gs.ebaocloud.sea.client.Model.PolicyModel
{
    /// <summary>
    /// java TimeStamp --> c# ?
    /// </summary>
    abstract class Policy
    {
        String insurerTenantCode;
        String channelTenantCode;
        long transId;
        long policyId;
        String quoteNo;
        String policyNo;
        String coreRefNo;
        String barcode;
        String prdtLine;
        String prdtCate;
        String prdtCode;//
        String prdtVersion;//
        DateTimeOffset proposalDate;//
        String billRefNo;
        DateTimeOffset effDate;//
        DateTimeOffset expDate;//
        byte policyStatus;
        long agreementId;
        String salesLicenseNo;
        String salesLicenseName;
        byte newOrRn;
        byte policySource;
        long insurerEntityId;
        long channelEntityId;
        String prevPolicyNo;
        DateTimeOffset issueDate;
        long issueUserId;
        String sminfo;

        Decimal app = Decimal.Zero;
        Decimal sgp = Decimal.Zero;
        Decimal snp = Decimal.Zero;
        Decimal agp = Decimal.Zero;
        Decimal anp = Decimal.Zero;

        List<Insured> insureds;//
        PolStakeholder policyholder;//
        List<PolStakeholder> insuredPersons;
        SimpleFee simpleFee;
        LastUWInfo lastUWInfo;
        List<MessageModel> uploadMapErrors;
        // ext
    }

    class SimpleFee
    {
        //List<FeeObject> feeAmounts;
        //List<TaxObject> taxAmounts;

        IDictionary<String, Decimal> discounts;
        IDictionary<String, Decimal> loadings;

        IDictionary<String, Decimal> discountRates;
        IDictionary<String, Decimal> loadingRates;

        Decimal snp;
        Decimal anp;
        Decimal app;
        Decimal agp;
    }

    

    class PolStakeholder
    {
        long policyId;
        long stakeholderId;
        byte isPolicyholder = 0;
        byte isSalesChannel = 0;
        byte isDriver = 0;
        byte isUnderwriter = 0;
        byte isInsuredPerson = 0;
        byte isDataEntryOperator = 0;
        byte isWatcher = 0;
        long refId;
        Byte indiOrOrg;
        String taxNo;
        int title;
        String name1;
        String name2;
        String name3;
        Byte idType;
        String idNo;
        String email;
        String mobile;
        String officeTel;
        String homeTel;
        StakeholderProfile profile;
    }

    class StakeholderProfile
    {
        String nationality;
        String occupation;
        String gender;
        String fax;
        String bankCode;
        String bankBranchCode;
        String bankAccNo;
        String bankAccName;
        String contactPersonName1;
        String contactPersonName2;
        String contactPersonName3;
        String contactTel;
        String contactMobile;
        String contactEmail;
        DateTime dob;
        String industryCate;
    }

    class LastUWInfo
    {
        byte decision;
        String declineReason;
        String comments;
        SimpleFee approvedPremium;
    }
}
