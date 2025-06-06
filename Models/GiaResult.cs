﻿using System;

namespace Ynost.Models;

public class GiaResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TeacherId { get; set; }        // ← добавить
    public string Subject { get; set; } = string.Empty;
    public string Group { get; set; } = string.Empty;
    public string TotalParticipants { get; set; } = string.Empty;
    public string PctMark5 { get; set; } = string.Empty;
    public string PctMark4 { get; set; } = string.Empty;
    public string PctMark3 { get; set; } = string.Empty;
    public string PctFail { get; set; } = string.Empty;
    public string AvgScore { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}
