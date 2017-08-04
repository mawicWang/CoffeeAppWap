//using Business;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;

//namespace XMS.UnitTest
//{


//    /// <summary>
//    ///This is a test class for UrlMappingHelperTest and is intended
//    ///to contain all UrlMappingHelperTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class UrlMappingHelperTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        // 
//        //You can use the following additional attributes as you write your tests:
//        //
//        //Use ClassInitialize to run code before running the first test in the class
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //Use ClassCleanup to run code after all tests in a class have run
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //Use TestInitialize to run code before running each test
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //Use TestCleanup to run code after each test has run
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///A test for GetMessageContent
//        ///</summary>
//        [TestMethod()]
//        public void GetMessageContentTest()
//        {
//            UrlMappingHelper target = new UrlMappingHelper(); // TODO: Initialize to an appropriate value
//            string orginalContent = @"千余人的热情参与，又有75位会员在一个月的吃喝后，轻松领走888 1888 5888 18888秘币红包，消费券、精美蛋糕，爱换啥换啥。2014年2月17日，第三波挑战将如约而至，别再沉默了，赶紧行动起来吧！</div>
//<div>
//	第2期获奖详情请见：<a href=""http://www.xiaomishu.com/shop/subject/detail_4_1128/?adw=Index"" target=""_blank"">http://www.xiaomishu.com/shop/subject/detail_4_1128/?adw=Index</a>";
//            string expected = string.Empty; // TODO: Initialize to an appropriate value
//            string actual;
//            actual = target.GetMessageContent(orginalContent);
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("Verify the correctness of this test method.");
//        }
//    }
//}
