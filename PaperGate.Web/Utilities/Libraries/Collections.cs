namespace PaperGate.Web.Utilities.Libraries;

public static class IconCollection
{
    public static List<KeyValue> GetIcons =
        [
            new(){Name ="تلگرام", Value="fab fa-telegram text-blue-500"},
new(){Name ="اینستاگرام", Value="fab fa-instagram text-pink-500"},
new(){Name ="فیسبوک", Value="fab fa-facebook text-blue-600"},
new(){Name ="واتس اپ", Value="fab fa-whatsapp text-green-500"},
new(){Name ="یوتوب", Value="fab fa-youtube text-red-600"},
new(){Name ="توئیتر", Value="fab fa-twitter text-sky-500"},
new(){Name ="تیک تاک", Value="fab fa-tiktok text-neutral-800"},
new(){Name ="لینکدین", Value="fab fa-linkedin text-blue-700"},
new(){Name ="آدرس فیزیکی", Value="fas fa-map-marker-alt text-gray-800"},
new(){Name ="شماره تلفن ثابت", Value="fas fa-phone text-blue-600"},
new(){Name ="شماره تلفن همراه", Value="fas fa-mobile-alt text-blue-600"},
new(){Name ="اسنپ چت", Value="fab fa-snapchat-ghost text-warning"},
new(){Name ="پینترست", Value="fab fa-pinterest text-danger"},
new(){Name ="ریددیت", Value="fab fa-reddit-alien text-orange-600"},
new(){Name ="دیسکورد", Value="fab fa-discord text-indigo-500"},
new(){Name ="گیت هاب", Value="fab fa-github text-dark"},
new(){Name ="مدیوم", Value="fab fa-medium text-gray-700"},
new(){Name ="اسلک", Value="fab fa-slack text-purple-600"},
new(){Name ="دریبل", Value="fab fa-dribbble text-pink-500"},
new(){Name ="بهینس", Value="fab fa-behance text-blue-600"},
new(){Name ="استک اورفلو", Value="fab fa-stack-overflow text-orange-500"},
new(){Name ="تاملر", Value="fab fa-tumblr text-blue-800"},
new(){Name ="اسکایپ", Value="fab fa-skype text-sky-500"},
new(){Name ="ایمیل", Value="fas fa-envelope text-secondary"},
new(){Name ="وبسایت", Value="fas fa-globe text-success"}
        ];
}
public class KeyValue
{
    public string Name { get; set; }
    public string Value { get; set; }
}