namespace App
{
    public interface ICustomerService
    {
        bool AddCustomer(CustomerAddCommand customerAddCommand);
    }
}
