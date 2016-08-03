using Coypu;
using NUnit.Framework;

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
        /// A test on the xpanxion.com website
        /// <remarks>
        /// Checks user input on the name and email fields of the contact page
        /// </remarks>
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
        /// Three tests on the npr.org website
        /// <remarks>
        ///     Click "Programs & Podcasts" and verify menu opens
        ///     Get a list of Objects that store each element and link in the "News & Conversations" column
        ///     Click the link in the "News & Conversations" column with the given text
        /// </remarks>
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