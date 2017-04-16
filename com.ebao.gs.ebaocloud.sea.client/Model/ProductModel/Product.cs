using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.ebao.gs.ebaocloud.sea.client.vmi.model.ProductModel
{
    class Product
    {
        String code;
        String name;
        String version;
        String lineOfBusiness;
        String prdtCate;
        Boolean openCover = false;
        String targetTypeOfCustomer;
        int defaultPOI = 12;
        Boolean renewable = true;

        // ???
        IDictionary<String, Object> additionalInfo;
        List<Coverage> coverages;
        List<Plan> plans;

        List<Limit> limits;
        List<Deductible> deductibles;
        List<AddOn> addOns;
        List<Discount> discounts;
        List<Loading> loadings;

        List<Clause> clauses;
        /// <summary>
        /// List Object Type ?
        /// </summary>
        IDictionary<String, List<string>> codeTables;
    }

    class Coverage
    {
        String code;
        String name;
        Boolean mandatory;
        Boolean primary = true;
        Boolean renewable = true;
        Boolean flat = false;
        String groupCode;
        String groupName;
        List<Coverage> interests;
        IDictionary<String, Object> additionalInfo;
    }

    class Plan
    {
        String code;
        String version;
        String name;
        Boolean editable = false;
        DateTime effectiveDate;
        DateTime expiryDate;
        List<Coverage> coverages;
        List<String> descriptions;
        IDictionary<String, Object> additionalInfo;

        // plan coverage
        private class Coverage
        {
            String code;
            Boolean @checked = true;
            Boolean editable = false;
            List<Interest> interests;
    }

        private class Interest
        {
            String code;
            Boolean @checked = true;
            Boolean editable = false;
            List<Interest> children;
        }
    }

    class Limit
    {
        String code;
        String name;
        List<ApplyTo> applyTos;
        IDictionary<String, Object> additionalInfo;
        List<SubLimit> subLimits;
}

    class SubLimit
    {
        Boolean editable = false;
        String per;
        String defaultValue;
        String code;
        Object options;
    }

    class ApplyTo
    {
        List<String> plans;
        List<String> coverages;
        List<String> interests;
    }

    class Deductible
    {
        String code;
        String name;
        List<ApplyTo> applyTos;
        IDictionary<String, Object> additionalInfo;
        List<SubDeductible> subDeductibles;
    }

    class SubDeductible
    {
        Boolean editable = false;
        String per;
        String defaultValue;
        String code;
        Object options;
    }


    class AddOn
    {
        String code;
        String name;
        String description;
        Decimal defaultValue;
        Boolean editable = false;
        IDictionary<String, Object> additionalInfo;
    }

    class Discount
    {
        String name;
        String code;
        Boolean editable = false;
        String description;
        Boolean @checked = false;
        String type;
        Boolean autoApply = false;
        Decimal defaultValue;
        Object options;
        IDictionary<String, Object> additionalInfo;
    }

    class Loading
    {
        String name;
        String code;
        Boolean editable = false;
        String description;
        String type;
        Boolean @checked = false;
        Boolean autoApply = false;
        Decimal defaultValue;
        Object options;
        IDictionary<String, Object> additionalInfo;
    }


    class Clause
    {
        String code;
        String name;
        String content;
        int level = -1;
        int typeOfClause = -1;
    }
}
