using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;
using Arun.Manglick.UI;

public partial class Trial4 : System.Web.UI.Page
{
    private const String Yes = "Y";
    private const String No = "N";
    private const String Monthly = "M";
    private const String Estimate = "C";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            rdoCreditLineNo.Checked = true;
            rdoEstimateMonthlyPayment1.Checked = true;            
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DebtOtherSettings dtoDebtOtherSettings = GetDebtOtherSettingsDto();
    }

    private void SetDebtOtherSettingsDefaultData()
    {
        DebtOtherSettings dtoDebtOtherSettings = null;
        
        #region Credit Line

        if (dtoDebtOtherSettings != null)
        {
            if (dtoDebtOtherSettings.IncludeAccountsCreditLine == No)
            {
                rdoCreditLineYes.Checked = false;
                rdoCreditLineNo.Checked = true;

                rdoSetMonthlyPayment1.Enabled = false;
                rdoEstimateMonthlyPayment1.Enabled = false;
                txtEstimateMonthlyPayment1.Enabled = false;

                if (dtoDebtOtherSettings.CreditLineType == Monthly)
                {
                    rdoSetMonthlyPayment1.Checked = true;
                    rdoEstimateMonthlyPayment1.Checked = false;
                }
                else if (dtoDebtOtherSettings.CreditLineType == Estimate)
                {
                    rdoSetMonthlyPayment1.Checked = false;
                    rdoEstimateMonthlyPayment1.Checked = true;
                }
            }
            else if (dtoDebtOtherSettings.IncludeAccountsCreditLine == Yes)
            {
                rdoCreditLineYes.Checked = true;
                rdoCreditLineNo.Checked = false;

                if (dtoDebtOtherSettings.CreditLineType == Monthly)
                {
                    rdoSetMonthlyPayment1.Checked = true;
                    rdoEstimateMonthlyPayment1.Checked = false;

                    rdoSetMonthlyPayment1.Enabled = true;
                    rdoEstimateMonthlyPayment1.Enabled = true;
                    txtEstimateMonthlyPayment1.Enabled = false;
                }
                else if (dtoDebtOtherSettings.CreditLineType == Estimate)
                {
                    rdoSetMonthlyPayment1.Checked = false;
                    rdoEstimateMonthlyPayment1.Checked = true;

                    rdoSetMonthlyPayment1.Enabled = true;
                    rdoEstimateMonthlyPayment1.Enabled = true;
                    txtEstimateMonthlyPayment1.Enabled = true;
                }
            }

            txtEstimateMonthlyPayment1.Text = dtoDebtOtherSettings.CreditLinePercent.ToString(CultureInfo.InvariantCulture);
        }

        #endregion

    }

    private DebtOtherSettings GetDebtOtherSettingsDto()
    {
        DebtOtherSettings dtoDebtOtherSettings = new DebtOtherSettings();
        String doubleText = String.Empty;

        #region Credit Line

        dtoDebtOtherSettings.IncludeAccountsCreditLine = Yes;
        if (rdoCreditLineNo.Checked)
        {
            dtoDebtOtherSettings.IncludeAccountsCreditLine = No;
        }

        dtoDebtOtherSettings.CreditLineType = Monthly;
        dtoDebtOtherSettings.CreditLinePercent = ConvertDouble(txtEstimateMonthlyPayment1.Text);
        if (rdoEstimateMonthlyPayment1.Checked)
        {
            dtoDebtOtherSettings.CreditLineType = Estimate;
        }

        #endregion

        return dtoDebtOtherSettings;
    }

    private double ConvertDouble(String value)
    {
        double res = 0;
        if (!String.IsNullOrEmpty(value))
        {
            res = Convert.ToDouble(value, CultureInfo.InvariantCulture);
        }

        return res;
    }

}
