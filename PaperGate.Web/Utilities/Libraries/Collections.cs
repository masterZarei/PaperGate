namespace PaperGate.Web.Utilities.Libraries;

public static class IconCollection
{
    public static List<KeyValue> GetIcons =
        [
            new(){Name ="تلگرام", Value="fab fa-telegram text-primary"},
            new(){Name ="اینستاگرام", Value="fab fa-instagram text-danger"},
            new(){Name ="فیسبوک", Value="fab fa-facebook text-info"},
            new(){Name ="واتس اپ", Value="fab fa-whatsapp text-success"},
            new(){Name ="یوتوب", Value="fab fa-youtube text-danger"},
            new(){Name ="توئیتر", Value="fab fa-twitter text-info"},
            new(){Name ="تیک تاک", Value="fab fa-tiktok text-dark"},
            new(){Name ="لینکدین", Value="fab fa-linkedin text-primary"},
            new(){Name ="آدرس فیزیکی", Value="fas fa-map-marker-alt text-white"},
            new() {Name ="شماره تلفن ثابت", Value="fa fa-phone text-primary"},
            new() {Name ="شماره تلفن همراه", Value="fa fa-mobile text-primary"}
        ];
}
public class KeyValue
{
    public string Name { get; set; }
    public string Value { get; set; }
}