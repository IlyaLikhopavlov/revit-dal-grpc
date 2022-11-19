namespace Revit.DAL.Exceptions
{
    public class RevitDataAccessException : BimCdException
    {
        public int Code { get; }
        
        public RevitDataAccessException(string message, int code = 0) : base(message)
        {
            Code = code;
        }

        public RevitDataAccessException()
        {
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {base.ToString()}";
        }
    }
}
