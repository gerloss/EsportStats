using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace EsportStats.Shared.Enums
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum e)
        {
            Type enumType = e.GetType();
            var value = Enum.GetName(enumType, e);
            MemberInfo member = enumType.GetMember(value)[0];

            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var output = ((DisplayAttribute)attributes[0]).Name;

            if (((DisplayAttribute)attributes[0]).ResourceType != null)
            {
                output = ((DisplayAttribute)attributes[0]).GetName();
            }

            return output;
        }
         
        public static string GetDescription(this Enum e)
        {
            Type enumType = e.GetType();
            var value = Enum.GetName(enumType, e);
            MemberInfo member = enumType.GetMember(value)[0];

            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var output = ((DisplayAttribute)attributes[0]).Description;

            if (((DisplayAttribute)attributes[0]).ResourceType != null)
            {
                output = ((DisplayAttribute)attributes[0]).GetDescription();
            }

            return output;
        }

        /// <summary>
        /// Thumbnail urls should be stored in the Description attribute.
        /// </summary>       
        public static string GetThumbnailUrl(this Enum e)
        {
            return GetDescription(e);
        }

        public static string GetShortName(this Enum e)
        {
            Type enumType = e.GetType();
            var value = Enum.GetName(enumType, e);
            MemberInfo member = enumType.GetMember(value)[0];

            var attributes = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var output = ((DisplayAttribute)attributes[0]).ShortName;

            if (((DisplayAttribute)attributes[0]).ResourceType != null)
            {
                output = ((DisplayAttribute)attributes[0]).GetShortName();
            }

            return output;
        }
    }
}
