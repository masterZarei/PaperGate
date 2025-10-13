using PaperGate.Core.Libraries.StaticValues;

namespace PaperGate.Core.Validations;
public static partial class Validation
{
    public static bool IsValidRole(string roleName) => roleName == Roles.AdminEndUser;
}
