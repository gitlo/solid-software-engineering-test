using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using App;
using Moq;

namespace UnitTestProject1
{
    [TestClass]
    public class CustomerServiceTest
    {
        private ICustomerService _customerService;
        private Mock<IDataAccessWrapper> _dataAccessWrapperMock = new Mock<IDataAccessWrapper>();        
        private Mock<ICompanyRepository> _companyRepository = new Mock<ICompanyRepository>();
        private Mock<ICreditCheck> _creditCheck = new Mock<ICreditCheck>();

        public CustomerServiceTest()
        {
            _customerService = new CustomerService(_dataAccessWrapperMock.Object, _companyRepository.Object, _creditCheck.Object);
        }

        [TestInitialize]
        public void Setup()
        {
            _companyRepository.Setup(p => p.GetById(1)).Returns(new Company { Id = 1 });
            _creditCheck.Setup(p => p.CheckCompany(It.IsAny<Company>(), It.IsAny<Customer>())).Returns(true);
        }

        [TestMethod]
        public void TestTestValidCustomer_SuccessfullyAddsCustomer()
        {
            var customerAddCommand = new CustomerAddCommand { Firstname = "Kez", Surname = "Nwichi", CompanyId = 1234, DateOfBirth = DateTime.Now.AddYears(-30), EmailAddress = "me@you.com", Id = 1 };
            _customerService.AddCustomer(customerAddCommand);
            _dataAccessWrapperMock.Verify(p => p.AddCustomer(It.IsAny<Customer>()));

        }

        [TestMethod]
        public void TestBelow21Customer_RejectsCustomer()
        {           

            var customerAddCommand = new CustomerAddCommand { Firstname = "Kez", Surname = "Nwichi", CompanyId = 1234, DateOfBirth = DateTime.Now.AddYears(-20), EmailAddress = "me@you.com", Id = 1 };
            _customerService.AddCustomer(customerAddCommand);
            _dataAccessWrapperMock.Setup(p => p.AddCustomer(It.IsAny<Customer>())).Throws(new Exception("Should not be called"));
        }
    }
}
