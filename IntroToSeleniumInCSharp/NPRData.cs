using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroToSeleniumInCSharp
{
    class NPRData
    {
        IWebElement element;
        public string href { get; set; }
        public string text { get; set; }

        public NPRData(IWebElement element)
        {
            this.element = element;
            href = element.FindElement(By.CssSelector("a")).GetAttribute("href");
            text = element.Text;
            
        }
        
        public string toString()
        {
            return (text + "\n" + "  ---> " + href);
        }
    }
}
