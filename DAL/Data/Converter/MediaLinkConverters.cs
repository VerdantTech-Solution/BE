using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using DAL.Data.Models;

namespace DAL.Data.Converters
{
    public static class MediaLinkConverters
    {
        // ==== OwnerType ====
        public static readonly ValueConverter<MediaOwnerType, string> OwnerTypeToString =
            new ValueConverter<MediaOwnerType, string>(
                v => ConvertOwnerTypeToString(v),
                v => ConvertStringToOwnerType(v));

        private static string ConvertOwnerTypeToString(MediaOwnerType v)
        {
            switch (v)
            {
                case MediaOwnerType.VendorCertificate: return "vendor_certificates";
                // case MediaOwnerType.ChatbotMessage:     return "chatbot_messages"; // nếu có enum này thì mở
                case MediaOwnerType.Product: return "products";
                case MediaOwnerType.ProductRegistration: return "product_registrations";
                case MediaOwnerType.ProductCertificate: return "product_certificates";
                case MediaOwnerType.ProductReview: return "product_reviews";
                case MediaOwnerType.ForumPost: return "forum_posts";
                default: throw new ArgumentOutOfRangeException(nameof(v), v, null);
            }
        }

        private static MediaOwnerType ConvertStringToOwnerType(string v)
        {
            switch (v)
            {
                case "vendor_certificates": return MediaOwnerType.VendorCertificate;
                // case "chatbot_messages":   return MediaOwnerType.ChatbotMessage;
                case "products": return MediaOwnerType.Product;
                case "product_registrations": return MediaOwnerType.ProductRegistration;
                case "product_certificates": return MediaOwnerType.ProductCertificate;
                case "product_reviews": return MediaOwnerType.ProductReview;
                case "forum_posts": return MediaOwnerType.ForumPost;
                default: throw new ArgumentOutOfRangeException(nameof(v), v, null);
            }
        }

        // ==== Purpose ====
        public static readonly ValueConverter<MediaPurpose, string> PurposeToString =
            new ValueConverter<MediaPurpose, string>(
                v => ConvertPurposeToString(v),
                v => ConvertStringToPurpose(v));

        private static string ConvertPurposeToString(MediaPurpose v)
        {
            switch (v)
            {
                case MediaPurpose.Front: return "front";
                case MediaPurpose.Back: return "back";
                case MediaPurpose.None: return "none";
                default: throw new ArgumentOutOfRangeException(nameof(v), v, null);
            }
        }

        private static MediaPurpose ConvertStringToPurpose(string v)
        {
            switch (v)
            {
                case "front": return MediaPurpose.Front;
                case "back": return MediaPurpose.Back;
                case "none": return MediaPurpose.None;
                default: throw new ArgumentOutOfRangeException(nameof(v), v, null);
            }
        }
    }
}
