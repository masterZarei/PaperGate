namespace PaperGate.Web.Utilities.Libraries;

public static class StaticValues
{
    #region File Paths
    //Images
    public const string PreferencesImagesPath = "wwwroot/Assets/Pictures/Papers/";


    #endregion
    #region Saving Size
    public const int MaxImageUploadSize = 20000000;
    #endregion
    public const int MaxImageUploadCount = 4;

    #region Path
    public const string LoginPath = "/Identity/Account/Login";
    public const string LogoutPath = "/Identity/Account/Logout";
    public const string RegisterPath = "/Identity/Account/Register";
    public const string AccessDeniedPath = "/Identity/Account/AccessDenied";
    #endregion
}