﻿using Domain.Enums;
using Domain.Models.Interfaces;
using System.Text.Json.Serialization;
namespace Domain.Models.Entities
{
    public class Admission : AdmissionDTO, IModel
    {
        [JsonPropertyName("id")]
        public Guid Guid { get; set; }
        
        [JsonIgnore]
        public Student Student { get; set; } = new();
        
        [JsonIgnore]
        public User User { get; set; } = new();

        [JsonIgnore]
        public List<AdmissionNote> Notes { get; set; } = [];
    }
    public class AdmissionDTO
    {
        [JsonPropertyName("studentId")]
        public Guid StudentGuid { get; set; }
        public string StudentSlug { get; set; } = string.Empty;
        [JsonPropertyName("userId")]
        public Guid UserGuid { get; set; }
        public string UserSlug { get; set; } = string.Empty;

        public string Program { get; set; } = string.Empty;
        public AdmissionStatus Status { get; set; }
        public DateTime ApplyDate { get; set; }

        public string Slug { get; set; } = string.Empty;
    }
}
