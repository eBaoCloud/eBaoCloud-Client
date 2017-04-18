using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using com.ebao.gs.ebaocloud.sea.seg.client.pub;
using System.Reflection;

namespace com.ebao.gs.ebaocloud.sea.seg.client.vmi.parameters
{
    public class Policy : BaseModel
    {
        public String productCode { get; set; }
        public String planCode { get; set; }

        public DateTime proposalDate { get; set; }
        public DateTime effectiveDate { get; set; }
        public DateTime expireDate { get; set; }
        public Insured insured { get; set; }
        public IndividualPolicyholder indiPolicyholder { get; set; }
        public OrganizationPolicyholder orgPolicyhodler { get; set; }
        public List<Document> documents { get; set; }
        public List<Driver> drivers { get; set; }
        public Boolean isPayerSameAsPolicyholder { get; set; }
        public Payer payer { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }

        public void Validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propList = type.GetProperties();

            foreach (PropertyInfo item in propList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (typeof(Boolean).IsInstanceOfType(value))
                {
                    Console.WriteLine("do nothing");
                }
                else if (typeof(List<Document>).IsInstanceOfType(value))
                {
                    List<Document> docList = (List<Document>)value;
                    if (docList.Count() > 0)
                    {
                        int index = 0;
                        foreach (Document doc in docList)
                        {
                            doc.Validate(index);
                            index++;
                        }
                    }
                }
                else if (typeof(List<Driver>).IsInstanceOfType(value))
                {
                    List<Driver> driverList = (List<Driver>)value;
                    if (driverList.Count() > 0)
                    {
                        int index = 0;
                        foreach (Driver driver in driverList)
                        {
                            driver.Validate(index);
                            index++;
                        }
                    }
                } else if (typeof(Insured).IsInstanceOfType(value))
                {
                    Insured insured = (Insured)value;
                    insured.validate();
                } else
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
                            continue ;
                        }

                        throw new Exception(name + " is required");
                    }

                }
            }
        }

        private Object GetValueOfProperty(string propName) {
            Type type = this.GetType();
            PropertyInfo propInfo = type.GetProperty(propName);
            return propInfo.GetValue(this, null);
        }
    }

    public class Driver
    {
        public String firstName;
        public String lastName;
        public DateTime birthday;
        public String occupation;

        public void Validate(int index = -1)
        {
            String extExceptionStr = "";
            if (index != -1)
            {
                extExceptionStr = String.Format(" at Driver list index [{0}]", index);
            }
            if (String.IsNullOrEmpty(this.firstName))
            {
                throw new Exception("Driver firstName is required" + extExceptionStr);
            } 
            if (String.IsNullOrEmpty(this.lastName))
            {  
                throw new Exception("Driver lastName is required" + extExceptionStr);
            }
        }
    }

    public class Payer
    {
        public String name;
        public InThaiAddress inThaiAddress;
        public OutThaiAddress outThaiAddress;
    }

    public class Document
    {
        public DocumentCategory category;
        public String name;
        public FileInfo file;

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


	public enum DocumentCategory {
		CAR_REGISTRATION = 1,
		DRIVING_LICENSE = 3
	}


    public class Insured
    {
        // optional
        public String vehicleCountry;
        public String vehicleProvince;
        public String vehicleColor;
        public Decimal vehicleAccessaryValue;
        public Decimal vehicleTotalValue;

        // required
        public VehicleGarageType vehicleGarageType;
        public String vehicleMakeName;
        public int vehicleModelYear;
        public int vehicleRegistrationYear;
        public String vehicleModelDescription;
        public VehicleUsage vehicleUsage;
        public String vehicleRegistrationNo;
        public String vehicleChassisNo;
        public String vehicleEngineNo;
        
        public void validate()
        {
            Type type = this.GetType();
            PropertyInfo[] propList = type.GetProperties();

            foreach (PropertyInfo item in propList)
            {
                String name = item.Name;
                Object value = item.GetValue(this, null);
                if (name == "vehicleCountry" || name == "vehicleProvince" || name == "vehicleColor" || name == "vehicleAccessaryValue" || name == "vehicleTotalValue")
                {
                    continue;
                }
                if (typeof(int).IsInstanceOfType(value) || typeof(VehicleUsage).IsInstanceOfType(value) || typeof(VehicleGarageType).IsInstanceOfType(value))
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
                throw new Exception(String.Format("In [{0}], ", type.Name) + name + " is required");
            }
        }
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
