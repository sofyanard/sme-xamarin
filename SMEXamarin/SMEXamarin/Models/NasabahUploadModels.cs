using System;
using System.Collections.Generic;
using System.Text;

namespace SMEXamarin.Models
{
    public class NasabahUploadResponse
    {
        public int Id { get; set; }
        public int NasabahId { get; set; }
        public string FileName { get; set; }
        public string Caption { get; set; }
        public string DownloadUrl { get; set; }
    }
}
