namespace CompProgEdu.Core.Security
{
    public abstract class Permission : StringEnum<Permission>
    {
        public const string View = "users.view";
        public const string Manage = "users.manage";
    }

    public abstract class Roles : StringEnum<Roles>
    {
        public const string GlobalAdmin = "Global Admin";
        public const string User = "User";
        public const string Instructor = "Instructor";
        public const string Student = "Student";
        public const string Deleted = "Deleted";
    }
}
