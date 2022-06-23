using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CompProgEdu.Core.Features.Response
{
    public class ValidateableResponse
	{
		private readonly IList<ErrorResponse> _errorMessages;

		public ValidateableResponse(IList<ErrorResponse> errors = null)
		{
			_errorMessages = errors ?? new List<ErrorResponse>();
		}

		public ValidateableResponse(string error, string fieldName)
		{
			_errorMessages = new List<ErrorResponse>();
			_errorMessages.Add(new ErrorResponse { Error = error, FieldName = fieldName});
		}

		public bool IsValidResponse => !_errorMessages.Any();

		public IReadOnlyCollection<ErrorResponse> Errors => new ReadOnlyCollection<ErrorResponse>(_errorMessages);
	}

	public class ValidateableResponse<TModel> : ValidateableResponse
		where TModel : class
	{
		public ValidateableResponse(TModel model, IList<ErrorResponse> validationErrors = null)
			: base(validationErrors)
		{
			Result = model;
		}
		public ValidateableResponse(string error, string fieldName)
			: base(error, fieldName)
		{
			Result = null;
		}
		public ValidateableResponse(IList<ErrorResponse> validationErrors)
		: base(validationErrors)
		{
			Result = null;
		}
		public TModel Result { get; }
	}

    public class PaginatedResponse<TModel> : ValidateableResponse where TModel : class
    {
        public PaginatedResponse(TModel model, IList<ErrorResponse> validationErrors = null)
            : base(validationErrors)
        {
            Result = model;
        }
        public PaginatedResponse(string error, string fieldName)
            : base(error, fieldName)
        {
            Result = null;
        }
        public PaginatedResponse(IList<ErrorResponse> validationErrors)
            : base(validationErrors)
        {
            Result = null;
        }

		public int TotalCount { get; set; }
        public TModel Result { get; }
	}

	public class ErrorResponse
	{
		public string FieldName { get; set; }
		public string Error { get; set; }
	}
}
