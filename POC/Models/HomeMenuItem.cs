﻿using System;
using System.Collections.Generic;
using System.Text;

namespace POC.Models
{
    public enum MenuItemType
    {
        Map,
        Browse,
        About
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
