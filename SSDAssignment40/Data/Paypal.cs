using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Data;
using System.Configuration;
using System.Web;
using SSDAssignment40;
using SSDAssignment40.Data;

namespace SSDAssignment40.Data
{
    public class Paypal
    {
        //Flag that determines the PayPal environment (live or sandbox)
        private const bool bSandbox = true;
        private const string CVV2 = "CVV2";

        // Live strings.
        private string pEndPointURL = "https://api-3t.paypal.com/nvp";
        private string host = "www.paypal.com";

        // Sandbox strings.
        private string pEndPointURL_SB = "https://api-3t.sandbox.paypal.com/nvp";
        private string host_SB = "www.sandbox.paypal.com";

        private const string SIGNATURE = "SIGNATURE";
        private const string PWD = "PWD";
        private const string ACCT = "ACCT";

        //Replace <Your API Username> with your API Username
        //Replace <Your API Password> with your API Password
        //Replace <Your Signature> with your Signature
        public string APIUsername = "<Your API Username>";
        private string APIPassword = "<Your API Password>";
        private string APISignature = "<Your Signature>";
        private string Subject = "";
        private string BNCode = "PP-ECWizard";

        //HttpWebRequest Timeout specified in milliseconds 
        private const int Timeout = 15000;
        private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

        public void SetCredentials(string Userid, string Pwd, string Signature)
        {
            APIUsername = Userid;
            APIPassword = Pwd;
            APISignature = Signature;
        }

        public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg)
        {
            if (bSandbox)
            {
                pEndPointURL = pEndPointURL_SB;
                host = host_SB;
            }

        }
        string returnURL = "https://localhost:44300/Checkout/CheckoutReview.aspx";
        string cancelURL = "https://localhost:44300/Checkout/CheckoutCancel.aspx";
    }
}
