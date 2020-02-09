namespace Blacksmith.Sql.Queries
{
    public interface IPaginableQuery : IOrderedQuery
    {
        Pagination Pagination { get; set; }
    }
}
