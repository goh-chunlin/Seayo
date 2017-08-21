using System;

namespace Seayo.Models
{
	public enum FileType
	{
		JPG, PNG, UNKNOWN
	}

    public class FileUpload : BaseEntity
    {
        public int ID { get; set; }

        public string Url { get; set; } 

		public FileType? Type { get; set; }
    }
}