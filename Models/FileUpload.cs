using System;

namespace Seayo.Models
{
	public enum FileType
	{
		JPG, PNG
	}

    public class FileUpload : BaseEntity
    {
        public int ID { get; set; }

        public byte[] File { get; set; } 

		public FileType? Type { get; set; }
    }
}