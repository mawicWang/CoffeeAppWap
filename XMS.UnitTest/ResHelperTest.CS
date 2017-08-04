//using Business;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Collections.Generic;
//using System.Web;

//namespace XMS.UnitTest
//{
    
    
//    /// <summary>
//    ///这是 ResHelperTest 的测试类，旨在
//    ///包含所有 ResHelperTest 单元测试
//    ///</summary>
//    [TestClass()]
//    public class ResHelperTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///获取或设置测试上下文，上下文提供
//        ///有关当前测试运行及其功能的信息。
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

//        #region 附加测试特性
//        // 
//        //编写测试时，还可使用以下特性:
//        //
//        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //使用 TestInitialize 在运行每个测试前先运行代码
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //使用 TestCleanup 在运行完每个测试后运行代码
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///GetBusinessHourByDiningTime 的测试
//        ///</summary>
//        [TestMethod()]
//        public void GetBusinessHourByDiningTimeTest()
//        {
//            string resId = string.Empty; // TODO: 初始化为适当的值
//            DateTime diningTime = new DateTime(); // TODO: 初始化为适当的值
//            BusinessHour expected = null; // TODO: 初始化为适当的值
//            List<BusinessHourDTO> actual;
//            resId = "E46K02R44568";
//            diningTime = DateTime.Now.AddHours(2);
//            actual = ResHelper.GetXbBusinessHourDTO(resId, diningTime.Date.AddDays(0), "");

//            //List<ResTable> rt = ResHelper.GetTableInfoListInRes(resId, diningTime);

//            //string dddd = XMS.Core.Json.JsonSerializer.Serialize(rt);

//            string json = XMS.Core.Json.JsonSerializer.Serialize(actual);
//            string ss = json;
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("验证此测试方法的正确性。");
//        }

//        /// <summary>
//        ///GetXMSResIdByApliayShopId 的测试
//        ///</summary>
//        //[TestMethod()]
//        //public void GetXMSResIdByApliayShopIdTest()
//        //{
//        //    string redirectUrl = HttpUtility.UrlEncode("http://m.57hao.com/AppWapXiaoBao/alipay/Qianbao/zfb_my_orders.html");
//        //    string GetOpenIdUrl = string.Format(AppSettingHelper.AlipayAuthorizeUrl.Replace("?", "") + "?app_id={0}&auth_skip=false&scope=auth_base&redirect_uri={1}", AppSettingHelper.AlipayAppId, redirectUrl);
//        //    string shopId = "100"; // TODO: 初始化为适当的值
//        //    string expected = string.Empty; // TODO: 初始化为适当的值
//        //    string actual = GetOpenIdUrl;
//        //    actual = ResHelper.GetXMSResIdByApliayShopId(shopId);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("验证此测试方法的正确性。");
//        //}
//    }
//}
