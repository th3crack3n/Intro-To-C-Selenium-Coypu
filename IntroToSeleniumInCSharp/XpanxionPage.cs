using Coypu;
using Coypu.NUnit.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace IntroToSeleniumInCSharp
{

    class XpanxionPage
    {
        private BrowserSession browser;

        private string
            home = "http://www.xpanxion.com",
            post = "/contact.html",
            name_field = "widgetu1680_input",
            email_field = "widgetu1684_input";

        public XpanxionPage(BrowserSession browser)
        {
            this.browser = browser;
            this.browser.Visit(home + post);
        }

        public void fillName(string name = "")
        {
            browser.FillIn(name_field).With(name);
        }

        public void fillEmail(string email = "")
        {
            browser.FillIn(email_field).With(email);
        }

        public void emptyFields()
        {
            fillName();
            fillEmail();
        }
        
        private string getNameValue()
        {
            return browser.FindField(name_field).Value;
        }

        private string getEmailValue()
        {
            return browser.FindField(email_field).Value;
        }

        public void verifyDataPresent(bool dataPresent)
        {
            // true if desiring data, false otherwise
            if (dataPresent == true)
            {
                Assert.IsNotEmpty(getNameValue());
                Assert.IsNotEmpty(getEmailValue());
            }
            else
            {
                Assert.IsEmpty(getNameValue());
                Assert.IsEmpty(getEmailValue());
            }
        }


    }
}
