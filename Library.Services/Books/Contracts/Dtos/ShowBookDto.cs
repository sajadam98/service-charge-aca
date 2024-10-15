namespace Library.Services.Books.Contracts.Dtos;

public class ShowBookDto
{
    public string Title { get; set; }
}

public record GetBookById(string Title);

