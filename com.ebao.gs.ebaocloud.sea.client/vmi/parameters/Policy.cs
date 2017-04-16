using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class Policy
    {
        public String productCode;
        public String productVersion;
        public DateTime proposalDate;
        public DateTime effectiveDate;
        public DateTime expireDate;
        public Insured insured;
        public IndividualPolicyholder indiPolicyholder;
        public OrganizationPolicyholder orgPolicyhodler;
        public List<Document> documents;
        public List<Driver> drivers;
        public Boolean isPayerSameAsPolicyholder;
        public Payer payer;
    }

    public class Driver
    {
        public String firstName;
        public String lastName;
        public DateTime birthday;
        public String occupation;
    }

    public class Payer
    {
        public String name;
        public InThaiAddress inThaiAddress;
        public OutThaiAddress outThaiAddress;
    }

    public class Document
    {
        public String category;
        public String name;
        public FileInfo file;
    }

    public class Insured
    {
        public String vehicleCountry;
        public String vehicleGarageType;
        public String vehicleProvince;
        public String vehicleMake;
        public String vehicleModel;
        public String vehicleYear;
        public String vehicleRegYear;
        public String vehicleDesc;
        public String vehicleGroup;
        public Decimal vehicleMarket;
        public int vehicleCapacity;
        public int vehicleNumOfSeats;
        public int vehicleTonnage;
        public String vehicleType;
        public String vehicleCode;
        public String vehicleUsage;
        public String vehicleRegistrationNo;
        public String vehicleChassisNo;
        public String vehicleColor;
        public String vehicleEngineNo;
    }

    public class InThaiAddress
    {
        public String street;
        public String province;
        public String district;
        public String subDistrict;
        public String postalCode;
    }

    public class OutThaiAddress
    {
        public String address;
        public String city;
        public String country;
    }
  

    public class IndividualPolicyholder
    {
        public String taxNo;
        public IndividualPrefix title;
        public String firstName;
        public String lastName;
        public String idType;
        public String idNo;
        //String email;
        public String mobile;
        //String officeTel;
        //String homeTel;
       // public String occupation;
        //public String nationality;
        // PolicyholderProfile profile;
        public InThaiAddress inThaiAddress;
        public OutThaiAddress outThaiAddress;
    }

    public class OrganizationPolicyholder
    {
        public OrganizationPrefix title;
        public String companyName;
        public String registrationNo;
        public String taxNo;
        public String branch;
        public DateTime registrationDate;
        public String industryCategory;
        public String contactPerson;
        public String contactPhoneNo;
        public InThaiAddress inThaiAddress;
        public OutThaiAddress outThaiAddress;
    }  

    public enum IndividualPrefix
    {
        Khun = 5,
        Mr = 6,
        Mrs = 7,
        Miss = 8
    }

    public enum OrganizationPrefix
    {
        Bank = 21,
        Group = 14,
        Warehouse = 16,
        Company = 1,
        University = 9
    }

}
