namespace ShowroomManagmentSystem.Utils
{
    public static class Constants
    {
        // Role Account
        public const int ROLE_USER     = 0;
        public const int ROLE_ADMIN    = 1;
        public const int ROLE_EMPLOYEE = 2;
        public const int ROLE_INVOICE  = 3;

        // Account Status
        public const int ACTIVE        = 0; // 0: Active    (Mở khóa)
        public const int LOCKED        = 1; // 1: Locked    (Tạm khóa) 
        public const int SUSPENDED     = 2; // 2: Suspended (Cấm)


    }
}