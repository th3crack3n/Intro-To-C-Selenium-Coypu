using Coypu;
using Coypu.NUnit.Matchers;
using NUnit.Framework;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IntroToSeleniumInCSharp
{
    [TestFixture]
    class TestSelenium
    {
        public static void Main(string[] args) { }

        static BrowserSession browser;

        enum OptionsNPR
        {
            Music,
            Topics,
            ProgramsAndPodcasts
        }

        static readonly string xpanxionNameField = "widgetu1680_input";
        static readonly string xpanxionEmailField = "widgetu1684_input";

        [TestFixtureSetUp]
        public void init()
        {
            browser = new BrowserSession(new SessionConfiguration
            {
                Browser = Coypu.Drivers.Browser.Chrome
            });

            browser.MaximiseWindow();
        }

        /// <summary>
        /// A test on the xpanxion.com website, specifically to check user input on the name and email fields of the contact page
        /// </summary>
        [Test]
        public static void testXpanxionForm()
        {
            browser.Visit("http://xpanxion.com/contact.html");

            // Add user info & verify
            browser.FillIn(xpanxionNameField).With("Sean McCracken");
            browser.FillIn(xpanxionEmailField).With("smccracken@xpanxion.com");
            verifyUserData(true);
        
            // Delete user info & verify
            browser.FillIn(xpanxionNameField).With("");
            browser.FillIn(xpanxionEmailField).With("");
            verifyUserData(false);
        }

        /// <summary>
        /// Verifies the name and email fields contents, either empty or not
        /// </summary>
        /// <param name="dataPresent">Whether data should be present</param>
        private static void verifyUserData(bool dataPresent)
        {
            // true if desiring data, false otherwise
            if (dataPresent == true)
            {
                Assert.IsNotEmpty(getNameValue());
                Assert.IsNotEmpty(getEmailValue()); 
            } else
            {
                Assert.IsEmpty(getNameValue());
                Assert.IsEmpty(getEmailValue());
            }
        }

        /// <summary>
        /// Gets the text value of the name field
        /// </summary>
        /// <returns>String value of name field</returns>
        private static string getNameValue()
        {
            return browser.FindField(xpanxionNameField).Value;
        }

        /// <summary>
        /// Gets the text value of the email field
        /// </summary>
        /// <returns>String value of the email field</returns>
        private static string getEmailValue()
        {
            return browser.FindField(xpanxionEmailField).Value;
        }


        /// <summary>
        /// Three tests on the npr.org website:
        ///     <list type="bullet">
        ///         <item>
        ///             <description>Click "Programs & Podcasts" and verify menu opens</description>
        ///         </item>
        ///         <item>
        ///             <description>Get a list of Objects that store each element and link in the "News & Conversations" column</description>
        ///         </item>
        ///         item>
        ///             <description>Click the link in the "News & Conversations" column with the given text</description>
        ///         </item>
        ///     </list>
        /// </summary>
        [Test]
        public static void testNPRdata()
        {
            browser.Visit("http://www.npr.org");

            // open a specific menu
            var place = OptionsNPR.ProgramsAndPodcasts;
            clickOption(NPROptionToClass(place));
            verifyMenuOpen(place);
            clickOption(NPROptionToClass(place));

            // get 'news & conversations' elements from 'programs & podcasts' menu 
            var data = getNewsAndConversationsNPR();
            outputNewsAndConversationsNPR(data);
            clickOption(NPROptionToClass(OptionsNPR.ProgramsAndPodcasts));

            // click specific 'news & conversations' element from 'programs & podcasts' menu
            clickByTextNC("All Things Considered");
            Assert.AreEqual(getURIByText(data, "all things considered"), browser.Location.AbsoluteUri);
        }

        /// <summary>
        /// Returns a string containing a css class, given an enum value from OptionsNPR
        /// </summary>
        /// <param name="option">A value from the OptionNPR enum</param>
        /// <returns>String value of a css class</returns>
        private static string NPROptionToClass(OptionsNPR option)
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

        /// <summary>
        /// Clicks an element found by providing a css class to the findOption() method
        /// </summary>
        /// <param name="option">A css class name</param>
        private static void clickOption(string option)
        {
            findOption(option).Click();
        }

        /// <summary>
        /// Verifies that the menu option provided by an enum value from OptionsNPR is open (chosen) 
        /// </summary>
        /// <param name="option">A css class name</param>
        private static void verifyMenuOpen(OptionsNPR option)
        {
            Assert.That(findOption(".ecosystem-news"), Shows.Css(" .chosen"));
        }

        /// <summary>
        /// Returns an ElementScope object with the first occurance of a given css class
        /// </summary>
        /// <param name="option">A css class name</param>
        /// <returns>An ElementScope object referencing a specific element</returns>
        private static ElementScope findOption(string option)
        {
            return browser.FindCss(option, Options.First);
        }

        /// <summary>
        /// Gathers data from the "News & Conversations" section of the "Programs & Podcasts" menu, and returns a list of NPRData objects referecing the element data
        /// </summary>
        /// <returns>A List of NPRData objects</returns>
        private static List<NPRData> getNewsAndConversationsNPR()
        {
            List<NPRData> data = new List<NPRData>();

            clickOption(NPROptionToClass(OptionsNPR.ProgramsAndPodcasts));
            var result = browser.FindCss(".group", Options.First).FindAllCss("li").ToList();

            foreach (SnapshotElementScope scope in result)
            {
                data.Add(new NPRData((IWebElement)scope.Native));
            }
        
            return data;
        }

        /// <summary>
        /// Prints the values in a List of NPRData objects to console
        /// </summary>
        /// <param name="data">A List of NPRData objects</param>
        private static void outputNewsAndConversationsNPR(List<NPRData> data)
        {
            foreach (NPRData d in data)
            {
                Console.WriteLine(d.toString());
            }
        }

        /// <summary>
        /// Clicks a link in the NPRData List given a text value of one of the elements
        /// </summary>
        /// <param name="data">A List of NPRData objects</param>
        /// <param name="text">A string reperesentation of a heading</param>
        public static void clickByTextNC(string text)
        {
            clickOption(NPROptionToClass(OptionsNPR.ProgramsAndPodcasts));
            browser.FindLink(text).Click();
        }

        /// <summary>
        /// Gets the URI of a NPRData object that matches the given text
        /// </summary>
        /// <param name="list">A List of NPRData objects</param>
        /// <param name="text">String value to match for heading text</param>
        /// <returns></returns>
        private static string getURIByText(List<NPRData> list, string text)
        {
            foreach (NPRData d in list)
            {
                if (d.text == text)
                {
                    return d.href;
                }
            }

            return null;
        }

        [TestFixtureTearDown]
        public static void destroy()
        {
            browser.Dispose();
        }
    }
}