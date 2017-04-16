using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class Policy
    {
        String prdtCode;
        String prdtVersion;
        DateTime proposalDate;
        DateTime effectiveDate;
        DateTime expireDate;
        Insured insured;
        Policyholder policyholder;
        List<Document> documents;
    }

    class Document
    {
        String category;
        String name;
        FileInfo file;
    }

    class Insured
    {
        String vehicleCountry;
        String vehicleGarageType;
        String vehicleProvince;
        String vehicleMake;
        String vehicleModel;
        String vehicleYear;
        String vehicleRegYear;
        String vehicleDesc;
        String vehicleGroup;
        Decimal vehicleMarket;
        int capacity;
        int numOfSeats;
        int tonnage;
        //String vehicleMarketGroup;
        String vehicleType;
        String vehicleCode;
        String vehicleUsage;
    }

  

    class Policyholder
    {
        Byte indiOrOrg;
        String taxNo;
        String title;
        String name1;
        String name2;
        String name3;
        String idType;
        String idNo;
        String email;
        String mobile;
        String officeTel;
        String homeTel;
        // PolicyholderProfile profile;
    }

    class PolicyholderProfile
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
}
