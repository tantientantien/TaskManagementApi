﻿using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi.Dtos
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Task ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title length cannot exceed 200 characters.")]
        public string? Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Description length cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Required(ErrorMessage = "UserId is required.")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; set; }
    }
}