using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
	public class OperationResult
	{
		public bool Succeeded { get; set; }
		public string Message { get; set; }
		public object Data { get; set; }
	}
}

