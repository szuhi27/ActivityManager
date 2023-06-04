﻿using System.ComponentModel.DataAnnotations;

namespace ActivityManager.Web.Models
{
    public class ActivityModel
    {
        public Guid Id { get; set; }
        [Required] public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public double? Duration { get; set; }
        public string? Note { get; set; }
    }
}
