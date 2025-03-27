using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;


namespace PRM392.Utils
{
    public static class Utilities
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void QuickLog(string text, string logPath)
        {
            var dirPath = Path.GetDirectoryName(logPath);

            if (string.IsNullOrWhiteSpace(dirPath))
                throw new ArgumentException($"Specified path \"{logPath}\" is invalid", nameof(logPath));

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);

            using var writer = File.AppendText(logPath);
            writer.WriteLine($"{DateTime.Now} - {text}");
        }

        public static string? GetCurrentUserId() => _httpContextAccessor?.HttpContext?.User.FindFirstValue(Claims.Subject);

        public static string GenerateSlug(string text, bool unique)
        {
            // Chuyển thành chữ thường
            text = text.ToLower();

            // Chuẩn hóa tiếng Việt thành không dấu
            text = RemoveDiacritics(text);

            // Xóa các ký tự không mong muốn, giữ lại a-z, 0-9, khoảng trắng và dấu '-'
            text = Regex.Replace(text, @"[^a-z0-9\s-]", "");

            // Thay nhiều khoảng trắng bằng 1 khoảng trắng
            text = Regex.Replace(text, @"\s+", " ").Trim();

            // Giới hạn độ dài slug
            text = text.Substring(0, text.Length <= 45 ? text.Length : 45).Trim();

            // Chuyển khoảng trắng thành dấu gạch ngang
            text = Regex.Replace(text, @"\s", "-");

            if (!unique) return text;

            // Thêm timestamp
            string ticks = DateTime.Now.Ticks.ToString();

            return $"{text}_{ticks}";
        }

        public static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
