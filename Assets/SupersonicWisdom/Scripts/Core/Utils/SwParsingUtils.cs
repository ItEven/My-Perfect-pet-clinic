using System;
using System.Globalization;

namespace SupersonicWisdomSDK
{
    internal class SwParsingUtils
    {
        #region --- Public Methods ---

        internal long TryParseLong(string longString, long defaultValue = default)
        {
            if (string.IsNullOrEmpty(longString)) return defaultValue;

            try
            {
                return long.Parse(longString);
            }
            catch (Exception e)
            {
                SwInfra.Logger.LogException(e, EWisdomLogType.Utils, $"Failed {nameof(long.TryParse)}");
            }

            return defaultValue;
        }
        
        internal float TryParseFloat(string floatString, float defaultValue = default)
        {
            if (floatString.SwIsNullOrEmpty()) return defaultValue;

            try
            {
                return float.Parse(floatString, NumberStyles.Any, CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                SwInfra.Logger.LogException(e, EWisdomLogType.Utils, $"Failed {nameof(float.TryParse)}");
            }

            return defaultValue;
        }
        
        internal float TryParseFloat(object value, float defaultValue = default)
        {
            if (value is string stringValue)
            {
                return TryParseFloat(stringValue, defaultValue);
            }
            return value is float floatValue ? floatValue : defaultValue;
        }
        
        internal int TryParseInt(string intString, int defaultValue = default)
        {
            if (string.IsNullOrEmpty(intString)) return defaultValue;

            try
            {
                return int.Parse(intString);
            }
            catch (Exception e)
            {
                SwInfra.Logger.LogException(e, EWisdomLogType.Utils, $"Failed {nameof(int.TryParse)}");
            }

            return defaultValue;
        }
        
        internal bool? TryParseBool(string boolString, bool? defaultValue = null)
        {
            var result = defaultValue;
        
            if (string.IsNullOrEmpty(boolString)) return result;
        
            if (boolString == "1" || boolString.ToLower() == "true" || boolString.ToLower() == "yes")
            {
                result = true;
            }
        
            if (boolString == "0" || boolString.ToLower() == "false" || boolString.ToLower() == "no")
            {
                result = false;
            }
        
            return result;
        }

        #endregion
    }
}