namespace SMS.Data
{
    public class DataConstants
    {
        public const int MinUsernameLength = 5;
        public const int MinUserPasswordLength = 6;
        public const int MaxUserLength = 20;

        public const string EmailValidator = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";

        //Product
        public const int MinNameLength = 4;

        public const decimal MinPrice = 0.05M;
        public const decimal MaxPrice = 1000M;
    }
}
