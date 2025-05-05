namespace EmployeeManagementLibrary.Dto;

public class BaseResponse<T>
	{
		public string Message { get; set; }
		public bool IsSuccessful { get; set; }
		public T Data { get; set; }
	}
