namespace App
{
    public class DataAccessWrapper
    {
        public void AddCustomer(Customer customer)
        {
            CustomerDataAccess.AddCustomer(customer);
        }            
    }
}
