namespace ChatSharp.Domain
{
    public class GenericResponse<T>
    {
        //generic
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public bool NotFound { get; set; }
        public T? Data { get; set; }

        //redirect
        public bool Unauthorized { get; set; }

        public GenericResponse<T> Success(T data, string message = "")
        {
            this.Data = data;
            this.IsValid = true;
            this.Message = message;

            return this;
        }
        
        public GenericResponse<T> Error(string message = "")
        {
            this.IsValid = false;
            this.Message = message;

            return this;
        }

        public GenericResponse<T> Success(T data)
        {
            this.Data = data;
            this.IsValid = true;

            return this;
        }

        public GenericResponse<T> NoFound()
        {
            this.NotFound = true;
            return this;
        }
    }
}
