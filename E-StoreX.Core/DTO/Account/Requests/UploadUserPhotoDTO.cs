﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Account.Requests
{
    public class UploadUserPhotoDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
