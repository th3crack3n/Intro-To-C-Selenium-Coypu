﻿using Coypu;
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
            this.browser.Visit(home);
        }

        public void resetPage()
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

        public class Data
        {
            IWebElement element;
            public string href { get; set; }
            public string text { get; set; }

            public Data(IWebElement element)
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

        public List<Data> getNewsAndConversationsNPR()
        {
            List<Data> data = new List<Data>();

            clickOption(OptionToClassName(OptionsNPR.ProgramsAndPodcasts));
            var result = findOption(".group").FindAllCss("li").ToList();

            foreach (SnapshotElementScope scope in result)
            {
                data.Add(new Data((IWebElement)scope.Native));
            }

            return data;
        }

        public void outputNewsAndConversationsNPR(List<Data> data)
        {
            foreach (Data d in data)
            {
                Console.WriteLine(d.toString());
            }
        }

        public void clickByTextNC(string text)
        {
            clickOption(OptionToClassName(OptionsNPR.ProgramsAndPodcasts));
            browser.FindLink(text).Click();
        }

        public string getURIByText(List<Data> list, string text)
        {
            foreach (Data d in list)
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