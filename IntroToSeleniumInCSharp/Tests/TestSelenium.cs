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
            XpanxionPage xpx = new XpanxionPage(browser);

            // Add user info & verify
            xpx.fillName("Sean McCracken");
            xpx.fillEmail("smccracken@xpanxion.com");
            xpx.verifyDataPresent(true);

            // Delete user info & verify  
            xpx.emptyFields();
            xpx.verifyDataPresent(false);
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
            NPRPage npr = new NPRPage(browser);

            // open a specific menu
            var classOption = npr.OptionToClassName(NPRPage.OptionsNPR.ProgramsAndPodcasts);
            npr.clickOption(classOption);
            npr.verifyMenuOpen();
            npr.resetPage();

            // get 'news & conversations' elements from 'programs & podcasts' menu 
            var data = npr.getNewsAndConversationsNPR();
            npr.outputNewsAndConversationsNPR(data);
            npr.resetPage();

            // click specific 'news & conversations' element from 'programs & podcasts' menu
            var text = "All Things Considered";
            npr.clickByTextNC(text);
            Assert.AreEqual(npr.getURIByText(data, text), browser.Location.AbsoluteUri);
        }

        [TestFixtureTearDown]
        public static void destroy()
        {
            browser.Dispose();
        }
    }
}