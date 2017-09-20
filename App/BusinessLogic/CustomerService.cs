using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App
{
    public class CustomerService : ICustomerService
    {
        private readonly IDataAccessWrapper _dataAccessWrapper;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICreditCheck _creditCheck;

        public CustomerService(IDataAccessWrapper dataAccessWrapper, ICompanyRepository companyRepository, ICreditCheck creditCheck)
        {
            _dataAccessWrapper = dataAccessWrapper;
            _companyRepository = companyRepository;
            _creditCheck = creditCheck;
        }

        public bool AddCustomer(CustomerAddCommand customerAddCommand)
        {
            if (string.IsNullOrEmpty(customerAddCommand.Firstname) || string.IsNullOrEmpty(customerAddCommand.Surname))
            {
                return false;
            }

            if (!customerAddCommand.EmailAddress.Contains("@") && !customerAddCommand.EmailAddress.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - customerAddCommand.DateOfBirth.Year;
            if (now.Month < customerAddCommand.DateOfBirth.Month || (now.Month == customerAddCommand.DateOfBirth.Month && now.Day < customerAddCommand.DateOfBirth.Day)) age--;

            if (age < 21)
            {
                return false;
            }

            var company = _companyRepository.GetById(customerAddCommand.CompanyId);

            var customer = new Customer
            {
                Company = company,
                DateOfBirth = customerAddCommand.DateOfBirth,
                EmailAddress = customerAddCommand.EmailAddress,
                Firstname = customerAddCommand.Firstname,
                Surname = customerAddCommand.Surname
            };

            if(_creditCheck.CheckCompany(company, customer))
            {
                _dataAccessWrapper.AddCustomer(customer);
                return true;
            }

            return false;           
        }        
    }
}
