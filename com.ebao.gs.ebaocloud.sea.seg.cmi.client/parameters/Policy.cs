using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using com.ebao.gs.ebaocloud.sea.seg.cmi.client.pub;
using com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters;

namespace com.ebao.gs.ebaocloud.sea.seg.cmi.client.parameters
{
	public class Policy : BaseModel
	{
		public String productCode { get; set; }
		public String productVersion { get; set; }

        public DateTime proposalDate { get; set; }
        public DateTime effectiveDate { get; set; }
        public DateTime expireDate { get; set; }
        public Insured insured { get; set; }
        public IndividualPolicyholder indiPolicyholder { get; set; }
        public OrganizationPolicyholder orgPolicyhodler { get; set; }
        public List<Document> documents { get; set; }
        public Payer payer { get; set; }
        public Boolean isPayerSameAsPolicyholder { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propertyList = type.GetProperties();
            foreach (PropertyInfo item in propertyList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (typeof(Boolean).IsInstanceOfType(value))
                {
                   // Console.WriteLine("do nothing");
                }
                else if (typeof(List<Document>).IsInstanceOfType(value))
                {
                    List<Document> documentList = (List<Document>)value;
                    if (documentList.Count > 0)
                    {
                        int index = 0;
                        foreach (Document doc in documentList)
                        {
                            doc.Validate(index);
                            index++;
                        }
                    }
                }
                else if (typeof(Insured).IsInstanceOfType(value))
                {
                    Insured insured = (Insured)value;
                    insured.Validate();
                }
                else if (typeof(IndividualPolicyholder).IsInstanceOfType(value))
                {
                    IndividualPolicyholder policyHolder = (IndividualPolicyholder)value;
                    policyHolder.Validate();
                }
                else if (typeof(OrganizationPolicyholder).IsInstanceOfType(value))
                {
                    OrganizationPolicyholder policyHolder = (OrganizationPolicyholder)value;
                    policyHolder.Validate();
                }
                else if (typeof(Payer).IsInstanceOfType(value))
                {
                    Payer payer = (Payer)value;
                    payer.Validate();
                }
                else
                {
                    if (value == null)
                    {
                        if (name == "indiPolicyholder")
                        {
                            Object orgPolicyhodlerValue = GetValueOfProperty("orgPolicyhodler");
                            if (orgPolicyhodlerValue == null)
                            {
                                throw new Exception("orgPolicyhodler or indiPolicyholder is required");
                            }
                            continue;
                        }
                        if (name == "orgPolicyhodler")
                        {
                            Object individualPolicyholderValue = GetValueOfProperty("indiPolicyholder");
                            if (individualPolicyholderValue == null)
                            {
                                throw new Exception("orgPolicyhodler or indiPolicyholder is required");
                            }
                            continue;
                        }
                        if (name == "payer")
                        {
                            Boolean isPayerSameAsPolicyholder = (Boolean)GetValueOfProperty("isPayerSameAsPolicyholder");
                            if (!isPayerSameAsPolicyholder)
                            {
                                throw new Exception("If you do not want to set up 'payer', you have to set 'isPayerSameAsPolicyholder' flag to 'true'. Otherwise, you must provide 'payer' info");
                            }
                            continue;
                        }

                        throw new Exception(name + " is required");
                    }

                }

            }
        }
    }

	public class Payer
	{
		public String name { get; set; }
        public InThaiAddress inThaiAddress { get; set; }
        public OutThaiAddress outThaiAddress { get; set; }

        public void Validate()
        {
            if (String.IsNullOrEmpty(this.name))
            {
                throw new Exception("In Payer name is required.");
            }

            if(this.inThaiAddress == null && this.outThaiAddress == null)
            {
                throw new Exception("In payer inThaiAddress or outThaiAddress is required");
            }
            // address validate

        }
    }

	public class Document
	{
		public DocumentCategory category { get; set; }
        public String name { get; set; }
        public FileInfo file { get; set; }

        public void Validate(int index = -1)
        {
            String exceptionStr = "";
            String extExceptionStr = "";
            if (index != -1)
            {
                extExceptionStr = String.Format(" at document list index [{0}]", index);
            }
            if ((int)this.category == 0)
            {
                exceptionStr = "Document name is required";

                throw new Exception(exceptionStr + extExceptionStr);
            }
            if (this.file == null)
            {
                exceptionStr = "Document file is required";
                throw new Exception(exceptionStr + extExceptionStr);
            }
        }
    }


	public enum DocumentCategory
	{
		CAR_REGISTRATION = 1,
		DRIVING_LICENSE = 3
	}


	public class Insured
	{
		public VehicleType vehicleType { get; set; }
        public VehicleSubType vehicleSubType { get; set; }

        public String vehicleMakeName { get; set; }
        public int vehicleModelYear { get; set; }
        public String vehicleModelDescription { get; set; }
        public int vehicleRegistrationYear { get; set; }
        public String vehicleRegistrationNo { get; set; }
        public String vehicleCountry { get; set; }
        public String vehicleProvince { get; set; }
        public VehicleUsage vehicleUsage { get; set; }
        public String vehicleChassisNo { get; set; }

        public String vehicleColor { get; set; }
        public String vehicleEngineNo { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propertyList = type.GetProperties();
            foreach (PropertyInfo item in propertyList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (name == "vehicleColor" || name == "vehicleEngineNo")
                {
                    continue;
                }
                if (typeof(int).IsInstanceOfType(value) || typeof(VehicleType).IsInstanceOfType(value) || typeof(VehicleSubType).IsInstanceOfType(value) || typeof(VehicleUsage).IsInstanceOfType(value))
                {
                    if ((int)value != 0)
                    {
                        continue;
                    }
                } else
                {
                    if (value != null)
                    {
                        continue;
                    }
                }
                throw new Exception(String.Format("In {0} {1} is reuqired.", type.Name, name));
            }
        }
    }

	//TODO
	public enum VehicleType {
		Sedan = 587,
	}

	public enum VehicleSubType {
		Car_Seat_up_to_7_people = 618,
		Sedan_Powered_by_electricity = 588

	}

	public class InThaiAddress
	{
		public String street { get; set; }
        public String province { get; set; }
        public String district { get; set; }
        public String subDistrict { get; set; }
        public String postalCode { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propertyList = type.GetProperties();
            foreach (PropertyInfo item in propertyList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (value == null)
                {
                    throw new Exception(String.Format("In [ {0} {1} is required] ", type.Name, name));
                }
            }
        }
    }

	public class OutThaiAddress
	{
		public String address { get; set; }
        public String city { get; set; }
        public String country { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propertyList = type.GetProperties();
            foreach (PropertyInfo item in propertyList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (value == null)
                {
                    throw new Exception(String.Format("In [ {0} {1} is required] ", type.Name, name));
                }
            }
        }
    }


	public class IndividualPolicyholder : BaseModel
	{
		public String taxNo { get; set; }
        public IndividualPrefix title { get; set; }
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String idType { get; set; }
        public String idNo { get; set; }
        public String mobile { get; set; }
        public InThaiAddress inThaiAddress { get; set; }
        public OutThaiAddress outThaiAddress { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propList = type.GetProperties();
            foreach (PropertyInfo item in propList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (typeof(IndividualPrefix).IsInstanceOfType(value))
                {
                    if ((int)value != 0)
                    {
                        continue;
                    }
                }
                else
                {
                    if (value == null)
                    {
                        if (name == "inThaiAddress")
                        {
                            Object outThaiAddress = GetValueOfProperty("outThaiAddress");
                            if (outThaiAddress == null)
                            {
                                throw new Exception("In individualPolicyholder inThaiAddress or outThaiAddress is required");
                            }
                            OutThaiAddress outThaiAddr = (OutThaiAddress)outThaiAddress;
                            outThaiAddr.Validate();
                            continue;
                        }

                        if (name == "outThaiAddress")
                        {
                            Object inThaiAddress = GetValueOfProperty("inThaiAddress");
                            if (inThaiAddress == null)
                            {
                                throw new Exception("In individualPolicyholder inThaiAddress or outThaiAddress is required");
                            }
                            InThaiAddress inThaiAddr = (InThaiAddress)inThaiAddress;
                            inThaiAddr.Validate();
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                throw new Exception(String.Format("In [ {0} ], ", type.Name) + name + " is required.");
            }
        }
    }

    public class OrganizationPolicyholder : BaseModel
    {
        public OrganizationPrefix title { get; set; }
        public String companyName { get; set; }
        public String registrationNo { get; set; }
        public String taxNo { get; set; }
        public String branch { get; set; }
        public DateTime registrationDate { get; set; }
        public String industryCategory { get; set; }
        public String contactPerson { get; set; }
        public String contactPhoneNo { get; set; }
        public InThaiAddress inThaiAddress { get; set; }
        public OutThaiAddress outThaiAddress { get; set; }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propList = type.GetProperties();
            foreach (PropertyInfo item in propList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (name == "registrationDate" || name == "industryCategory" || name == "contactPerson" || name == "contactPhoneNo") continue;
                if (typeof(OrganizationPrefix).IsInstanceOfType(value))
                {
                    if ((int)value != 0)
                    {
                        continue;
                    }
                }
                else
                {
                    if (value == null)
                    {
                        if (name == "inThaiAddress")
                        {
                            Object outThaiAddress = GetValueOfProperty("outThaiAddress");
                            if (outThaiAddress == null)
                            {
                                throw new Exception("In OrganizationPolicyholder inThaiAddress or outThaiAddress is required");
                            }
                            OutThaiAddress outThaiAddr = (OutThaiAddress)outThaiAddress;
                            outThaiAddr.Validate();
                            continue;
                        }

                        if (name == "outThaiAddress")
                        {
                            Object inThaiAddress = GetValueOfProperty("inThaiAddress");
                            if (inThaiAddress == null)
                            {
                                throw new Exception("In OrganizationPolicyholder inThaiAddress or outThaiAddress is required");
                            }
                            InThaiAddress inThaiAddr = (InThaiAddress)inThaiAddress;
                            inThaiAddr.Validate();
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
                throw new Exception(String.Format("In [ {0} ], ", type.Name) + name + " is required.");
            }
        }
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
