﻿using System;

namespace Ynost.Models;

public class SelfDeterminationActivity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TeacherId { get; set; }        // ← добавить
    public string Level { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;
}
