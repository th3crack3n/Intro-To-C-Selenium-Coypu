using Coypu;
using Coypu.NUnit.Matchers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using OpenQA.Selenium;

namespace IntroToSeleniumInCSharp
{

    class NPRPage
    {
        private BrowserSession browser;

        public enum OptionsNPR
        {
            Music,
            Topics,
            ProgramsAndPodcasts
        }

        private string home = "http://www.npr.org";

        public NPRPage(BrowserSession browser)
        {
            this.browser = browser;
        }

        public void load()
        {
            browser.Visit(home);
        }

        public string OptionToClassName(OptionsNPR option)
        {
            string selected = null;

            switch (option)
            {
                case OptionsNPR.Music:
                    selected = ".music";
                    break;
                case OptionsNPR.Topics:
                    selected = ".topics";
                    break;
                case OptionsNPR.ProgramsAndPodcasts:
                    selected = ".programs-podcasts";
                    break;
            }

            return selected;
        }

        public void clickOption(string option)
        {
            findOption(option).Click();
        }

        public void verifyMenuOpen()
        {
            Assert.That(findOption(".ecosystem-news"), Shows.Css(" .chosen"));
        }

        public ElementScope findOption(string option)
        {
            return browser.FindCss(option, Options.First);
        }

        public List<NPRData> getNewsAndConversationsNPR()
        {
            List<NPRData> data = new List<NPRData>();

            clickOption(OptionToClassName(OptionsNPR.ProgramsAndPodcasts));
            var result = findOption(".group").FindAllCss("li").ToList();

            foreach (SnapshotElementScope scope in result)
            {
                data.Add(new NPRData((IWebElement)scope.Native));
            }

            return data;
        }

        public void outputNewsAndConversationsNPR(List<NPRData> data)
        {
            foreach (NPRData d in data)
            {
                Console.WriteLine(d.toString());
            }
        }

        public void clickByTextNC(string text)
        {
            clickOption(OptionToClassName(OptionsNPR.ProgramsAndPodcasts));
            browser.FindLink(text).Click();
        }

        public string getURIByText(List<NPRData> list, string text)
        {
            foreach (NPRData d in list)
            {
                if (d.text == text.ToLower())
                {
                    return d.href;
                }
            }

            return null;
        }

    }
}