using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Banking.cs
{
    public static class Ofx
    {
        public class Options
        {
            public enum AccountType
            {
                CreditCard,
                Investment,
                Checking,
                Other
            }

            public string Username { get; set; }
            public string Password { get; set; }
            public string AccountId { get; set; }
            public AccountType ActType { get; set; }
            public string Fid { get; set; }
            public string FidOrg { get; set; }
            public string BankId { get; set; }
            public string Url { get; set; }
            public string OfxVersion { get; set; }
            public string App { get; set; }
            public string AppVersion { get; set; }
            public string BrokerId { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }

        }
        public static string CreateRequest(Options options)
        {
            //TODO join ofxReq and ofxReqCC not sure why I seperated in the first place
            //Request for Bank statement
            string reqStr = "";

            if (options.ActType == Options.AccountType.Investment)
            {
                reqStr = GetOfxHeaders(options) +
                  "<OFX>" +
                  GetSignOnMsg(options) +
                  "<INVSTMTMSGSRQV1>" +
                  "<INVSTMTTRNRQ>" +
                  "<TRNUID>" + Guid.NewGuid().OfxFormat() +
                  "<CLTCOOKIE>" + RandomIdentifier(5) +
                  "<INVSTMTRQ>" +
                  "<INVACCTFROM>" +
                  "<BROKERID>" + options.BrokerId+
                  "<ACCTID>" + options.AccountId +
                  "</INVACCTFROM>" +
                  "<INCTRAN>" +
                  "<DTSTART>" + options.Start.OfxFormat() +
                  "<INCLUDE>Y</INCTRAN>" +
                  "<INCOO>Y" +
                  "<INCPOS>" +
                  "<INCLUDE>Y" +
                  "</INCPOS>" +
                  "<INCBAL>Y" +
                  "</INVSTMTRQ>" +
                  "</INVSTMTTRNRQ>" +
                  "</INVSTMTMSGSRQV1>" +
                  "</OFX>";
                } else
                if (options.ActType != Options.AccountType.CreditCard) {
                  reqStr = GetOfxHeaders(options) +
                    "<OFX>" +
                    GetSignOnMsg(options) +
                    "<BANKMSGSRQV1>" +
                    "<STMTTRNRQ>" +
                    "<TRNUID>" + RandomIdentifier(32) +
                    "<CLTCOOKIE>" + RandomIdentifier(5) +
                    "<STMTRQ>" +
                    "<BANKACCTFROM>" +
                    "<BANKID>" + options.BankId +
                    "<ACCTID>" + options.AccountId +
                    "<ACCTTYPE>" + options.ActType.ToString().ToUpper()+
                    "</BANKACCTFROM>" +
                    "<INCTRAN>" +
                    "<DTSTART>" + options.Start.OfxFormat()+
                    "<DTEND>" + options.End.OfxFormat() +
                    "<INCLUDE>Y</INCTRAN>" +
                    "</STMTRQ>" +
                    "</STMTTRNRQ>" +
                    "</BANKMSGSRQV1>" +
                    "</OFX>";
                } else {
                //  //Request for CreditCard Statement
                  reqStr = GetOfxHeaders(options) +
                    "<OFX>" +
                    GetSignOnMsg(options) +
                    "<CREDITCARDMSGSRQV1>" +
                    "<CCSTMTTRNRQ>" +
                    "<TRNUID>" + RandomIdentifier(32) +
                    "<CLTCOOKIE>" + RandomIdentifier(5) +
                    "<CCSTMTRQ>" +
                    "<CCACCTFROM>" +
                    "<ACCTID>" + options.AccountId +
                    "</CCACCTFROM>" +
                    "<INCTRAN>" +
                    "<DTSTART>" + options.Start.OfxFormat() +
                    "<INCLUDE>Y</INCTRAN>" +
                    "</CCSTMTRQ>" +
                    "</CCSTMTTRNRQ>" +
                    "</CREDITCARDMSGSRQV1>" +
                    "</OFX>";
                  }

            return reqStr;
        }

        public static string GetOfxHeaders(Options options)
        {

            return "OFXHEADER:100\n" +
                   "DATA:OFXSGML\n" +
                   "VERSION:" + options.OfxVersion + "\n" +
                   "SECURITY:NONE\n" +
                   "ENCODING:USASCII\n" +
                   "CHARSET:1252\n" +
                   "COMPRESSION:NONE\n" +
                   "OLDFILEUID:NONE\n" +
                   "NEWFILEUID:" + Guid.NewGuid().ToString("N") + "\n" +
                   "\n";
        }

        public static string GetSignOnMsg(Options options)
        {
            return "<SIGNONMSGSRQV1>" +
                  "<SONRQ>" +
                  "<DTCLIENT>" + options.End.OfxFormat() +
                  "<USERID>" + options.Username +
                  "<USERPASS>" + options.Password +
                  "<LANGUAGE>ENG" +
                  "<FI>" +
                  "<ORG>" + options.FidOrg +
                  "<FID>" + options.Fid +
                  "</FI>" +
                  "<APPID>" + options.App +
                  "<APPVER>" + options.AppVersion +
                  "</SONRQ>" +
                  "</SIGNONMSGSRQV1>";
        }

        public static string RandomIdentifier(int length)
        {
            var chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray()); 
        }
    }
}
