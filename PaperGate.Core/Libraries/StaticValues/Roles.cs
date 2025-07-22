namespace PaperGate.Core.Libraries.StaticValues;

public class Roles
{
    //ادمین
    //Full Access To Every section 
    public const string AdminEndUser = "Admin";
    //دانشجو
    //Limited Access
/*    public const string StudentEndUser = "Student";
*/
    public static Dictionary<int, string> GetRoles => new()
    {
/*        {1,StudentEndUser},
*/        {1,AdminEndUser},
    };
}
