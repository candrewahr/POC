﻿using System;
using System.Text.RegularExpressions;

namespace POC.Extensions
{
    static class PlacemarkExtensions
    {
        public static string GetFullStateName(this string adminArea)
        {
            switch (adminArea)
            {
                case "AL":
                    return "Alabama";

                case "AK":
                    return "Alaska";

                case "AS":
                    return "American Samoa";

                case "AZ":
                    return "Arizona";

                case "AR":
                    return "Arkansas";

                case "CA":
                    return "California";

                case "CO":
                    return "Colorado";

                case "CT":
                    return "Connecticut";

                case "DE":
                    return "Delaware";

                case "DC":
                    return "District of Columbia";

                case "FM":
                    return "FEDERATED STATES OF MICRONESIA";

                case "FL":
                    return "Florida";

                case "GA":
                    return "Georgia";

                case "GU":
                    return "Guam";

                case "HI":
                    return "Hawaii";

                case "ID":
                    return "Idaho";

                case "IL":
                    return "Illinois";

                case "IN":
                    return "Indiana";

                case "IA":
                    return "Iowa";

                case "KS":
                    return "Kansas";

                case "KY":
                    return "Kentucky";

                case "LA":
                    return "Louisiana";

                case "ME":
                    return "Maine";

                case "MH":
                    return "Marshall Islands";

                case "MD":
                    return "Maryland";

                case "MA":
                    return "Massachusetts";

                case "MI":
                    return "Michigan";

                case "MN":
                    return "Minnesota";

                case "MS":
                    return "Mississippi";

                case "MO":
                    return "Missouri";

                case "MT":
                    return "Montana";

                case "NE":
                    return "Nebraska";

                case "NV":
                    return "Nevada";

                case "NH":
                    return "New Hampshire";

                case "NJ":
                    return "New Jersey";

                case "NM":
                    return "New Mexico";

                case "NY":
                    return "New York";

                case "NC":
                    return "North Carolina";

                case "ND":
                    return "North Dakota";

                case "MP":
                    return "Northern Mariana Islands";

                case "OH":
                    return "Ohio";

                case "OK":
                    return "Oklahoma";

                case "OR":
                    return "Oregon";

                case "PW":
                    return "Palau";

                case "PA":
                    return "Pennsylvania";

                case "PR":
                    return "Puerto Rico";

                case "RI":
                    return "Rhode Island";

                case "SC":
                    return "South Carolina";

                case "SD":
                    return "South Dakota";

                case "TN":
                    return "Tennessee";

                case "TX":
                    return "Texas";

                case "UT":
                    return "Utah";

                case "VT":
                    return "Vermont";

                case "VI":
                    return "Virgin Islands";

                case "VA":
                    return "Virginia";

                case "WA":
                    return "Washington";

                case "WV":
                    return "West Virginia";

                case "WI":
                    return "Wisconsin";

                case "WY":
                    return "Wyoming";
            }

            throw new Exception("Not Available");
        }

        public static string SplitCamelCase(this string str)
        {
            return Regex.Replace(
                Regex.Replace(
                    str,
                    @"(\P{Ll})(\P{Ll}\p{Ll})",
                    "$1 $2"
                ),
                @"(\p{Ll})(\P{Ll})",
                "$1 $2"
            );
        }
    }
}