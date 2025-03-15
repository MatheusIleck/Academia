namespace AcademiaBackEnd.Extensions
{
    public interface IEndPoint
    {
        static abstract void Map(IEndpointRouteBuilder app);
    }
}
