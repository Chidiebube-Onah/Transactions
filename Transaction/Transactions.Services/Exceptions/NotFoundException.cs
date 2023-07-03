namespace SchMgr_FUPRE.Services.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        protected NotFoundException(string message) : base(message)
        {

        }
    }
}
