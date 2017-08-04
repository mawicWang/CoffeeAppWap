//using Business;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using XMS.Core;

//namespace XMS.UnitTest
//{
    
    
//    /// <summary>
//    ///这是 OrderHelperTest 的测试类，旨在
//    ///包含所有 OrderHelperTest 单元测试
//    ///</summary>
//    [TestClass()]
//    public class OrderHelperTest
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
//        ///AddOrUpdateOrder 的测试
//        ///</summary>
//        //[TestMethod()]
//        //public void AddOrUpdateOrderTest()
//        //{
//        //    ResOrder order = null; // TODO: 初始化为适当的值

//        //    string expected = string.Empty; // TODO: 初始化为适当的值
//        //    string actual;
//        //    order = new ResOrder();
//        //    string opName = "alipayqianbao";
//        //    ResOrder orderEntity = new ResOrder();
//        //    orderEntity.ResID = "AESH10000590";
//        //    orderEntity.TableUUIDs = "TBkjqdtpdjQtWSUo8muOMgZg";
//        //    orderEntity.DiningTime = DateTime.Now.AddHours(3);
//        //    orderEntity.DiningPeopleNum = 10;
//        //    orderEntity.OrderCreatorId = "7023401";
//        //    orderEntity.OrderCreator = opName;
//        //    orderEntity.OrderCreatorDesc = opName + "支付宝虚拟会员";
//        //    orderEntity.OrderMan = "张";
//        //    orderEntity.OrderManSex = 0;
//        //    orderEntity.OrderManTelphone = "13589809773";

//        //    orderEntity.CustomerName = "张";
//        //    orderEntity.CustomerSex = 1;
//        //    orderEntity.CustomerContact = "13589809773";
//        //    orderEntity.Notes = "测试";
//        //    orderEntity.OrderUUID = "";

//        //    actual = OrderHelper.AddOrUpdateOrder(orderEntity, opName);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("验证此测试方法的正确性。");
//        //}

//        /// <summary>
//        ///GetXBOrderInfoByOrderUUID 的测试
//        ///</summary>
//        //[TestMethod()]
//        //public void GetXBOrderInfoByOrderUUIDTest()
//        //{
//        //    OrderHelper target = new OrderHelper(); // TODO: 初始化为适当的值
//        //    string resId = "AESH10000590"; // TODO: 初始化为适当的值
//        //    string orderUUID = "d1b8c05f-694d-4706-b7a9-fcb95b9fc346"; // TODO: 初始化为适当的值
//        //    OrderInfoDTO expected = null; // TODO: 初始化为适当的值
//        //    OrderInfoDTO actual;
//        //    actual = OrderHelper.GetAlipayOrderInfoByOrderUUID(resId, orderUUID);

//        //    string orderJson = XMS.Core.Json.JsonSerializer.Serialize(actual);
//        //    string dd = orderJson;
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("验证此测试方法的正确性。");
//        //}

//        /// <summary>
//        ///GetOrderInfoList 的测试
//        ///</summary>
//        [TestMethod()]
//        public void GetOrderInfoListTest()
//        {
//            OrderHelper target = new OrderHelper(); // TODO: 初始化为适当的值
//            string user = "7023401"; // TODO: 初始化为适当的值
//            int startIndex = 0; // TODO: 初始化为适当的值
//            int orderCount = 10; // TODO: 初始化为适当的值
//            QueryResult<OrderInfoDTO> expected = null; // TODO: 初始化为适当的值
//            QueryResult<OrderInfoDTO> actual;
//            actual = OrderHelper.GetOrderInfoList("7024101", "AESH10001152", 1, 6);
//            string orderList = XMS.Core.Json.JsonSerializer.Serialize(actual);
//            string dd = orderList;
//            Assert.AreEqual(expected, actual);
//            Assert.Inconclusive("验证此测试方法的正确性。");
//        }

//        /// <summary>
//        ///CancleOrder 的测试
//        ///</summary>
//        //[TestMethod()]
//        //public void CancleOrderTest()
//        //{
//        //    string orderUUID = "921f5190-5eb1-45f0-93d8-40d9de6f2b59"; // TODO: 初始化为适当的值
//        //    string opName = string.Empty; // TODO: 初始化为适当的值
//        //    OrderHelper.CancleOrder(orderUUID, opName);
//        //    Assert.Inconclusive("无法验证不返回值的方法。");
//        //}

//        /// <summary>
//        ///RecoveryOrder 的测试
//        ///</summary>
//        //[TestMethod()]
//        //public void RecoveryOrderTest()
//        //{
//        //    string orderUUID = "921f5190-5eb1-45f0-93d8-40d9de6f2b59"; // TODO: 初始化为适当的值
//        //    string expected = string.Empty; // TODO: 初始化为适当的值
//        //    string actual="";
//        //    OrderHelper.RecoveryOrder(orderUUID);
//        //    Assert.AreEqual(expected, actual);
//        //    Assert.Inconclusive("验证此测试方法的正确性。");
//        //}
//    }
//}
